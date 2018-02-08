using Ltchina.Core.Weixin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Ltchina.Core.Weixin.CommonAPI
{
    /// <summary>
    /// GetMenu返回的Json结果
    /// </summary>
    public class GetMenuResult
    {
        public ButtonGroup menu { get; set; }

        public GetMenuResult()
        {
            menu = new ButtonGroup();
        }
    }
    /// <summary>
    /// 获取菜单时候的完整结构，用于接收微信服务器返回的Json信息
    /// </summary>
    public class GetMenuResultFull : WxJsonResult
    {
        public MenuFull_ButtonGroup menu { get; set; }
    }

    public class MenuFull_ButtonGroup
    {
        public List<MenuFull_RootButton> button { get; set; }
    }

    public class MenuFull_RootButton
    {
        public string type { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public List<MenuFull_RootButton> sub_button { get; set; }
    }

    public static class Menu
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public static WxJsonResult CreateMenu(string accessToken, ButtonGroup buttonData)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            return CommonJsonSend.Send(accessToken, urlFormat, buttonData);
        }

        /// <summary>
        /// 获取当前菜单，如果菜单不存在，将返回null
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static GetMenuResult GetMenu(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);

            var jsonString = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);

            GetMenuResult finalResult;
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                var jsonResult = js.Deserialize<GetMenuResultFull>(jsonString);
                if (jsonResult.menu == null || jsonResult.menu.button.Count == 0)
                {
                    throw new Exception(jsonResult.errmsg);
                }

                finalResult = GetMenuFromJsonResult(jsonResult);
            }
            catch
            {
                finalResult = null;
            }

            return finalResult;
        }

        // <summary>
        /// 根据微信返回的Json数据得到可用的GetMenuResult结果
        /// </summary>
        /// <param name="resultFull"></param>
        /// <returns></returns>
        public static GetMenuResult GetMenuFromJsonResult(GetMenuResultFull resultFull)
        {
            GetMenuResult result = null;
            try
            {
                //重新整理按钮信息
                ButtonGroup bg = new ButtonGroup();
                foreach (var rootButton in resultFull.menu.button)
                {
                    if (rootButton.name == null)
                    {
                        continue;//没有设置一级菜单
                    }
                    var availableSubButton = rootButton.sub_button.Count(z => !string.IsNullOrEmpty(z.name));//可用二级菜单按钮数量
                    if (availableSubButton == 0)
                    {
                        //底部单击按钮
                        if (rootButton.type == null ||
                            (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                            && string.IsNullOrEmpty(rootButton.key)))
                        {
                            throw new Exception("单击按钮的key不能为空！");
                        }

                        if (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                        {
                            //点击
                            bg.button.Add(new SingleClickButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                        {
                            //URL
                            bg.button.Add(new SingleViewButton()
                            {
                                name = rootButton.name,
                                url = rootButton.url,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("LOCATION_SELECT", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出地理位置选择器
                            bg.button.Add(new SingleLocationSelectButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("PIC_PHOTO_OR_ALBUM", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出拍照或者相册发图
                            bg.button.Add(new SinglePicPhotoOrAlbumButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("PIC_SYSPHOTO", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出系统拍照发图
                            bg.button.Add(new SinglePicSysphotoButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("PIC_WEIXIN", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出微信相册发图器
                            bg.button.Add(new SinglePicWeixinButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else if (rootButton.type.Equals("SCANCODE_PUSH", StringComparison.OrdinalIgnoreCase))
                        {
                            //扫码推事件
                            bg.button.Add(new SingleScancodePushButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else
                        {
                            //扫码推事件且弹出“消息接收中”提示框
                            bg.button.Add(new SingleScancodeWaitmsgButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                    }
                    else
                    {
                        //底部二级菜单
                        var subButton = new SubButton(rootButton.name);
                        bg.button.Add(subButton);

                        foreach (var subSubButton in rootButton.sub_button)
                        {
                            if (subSubButton.name == null)
                            {
                                continue; //没有设置菜单
                            }

                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                                && string.IsNullOrEmpty(subSubButton.key))
                            {
                                throw new Exception("单击按钮的key不能为空！");
                            }


                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                            {
                                //点击
                                subButton.sub_button.Add(new SingleClickButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                            {
                                //URL
                                subButton.sub_button.Add(new SingleViewButton()
                                {
                                    name = subSubButton.name,
                                    url = subSubButton.url,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("LOCATION_SELECT", StringComparison.OrdinalIgnoreCase))
                            {
                                //弹出地理位置选择器
                                subButton.sub_button.Add(new SingleLocationSelectButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("PIC_PHOTO_OR_ALBUM", StringComparison.OrdinalIgnoreCase))
                            {
                                //弹出拍照或者相册发图
                                subButton.sub_button.Add(new SinglePicPhotoOrAlbumButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("PIC_SYSPHOTO", StringComparison.OrdinalIgnoreCase))
                            {
                                //弹出系统拍照发图
                                subButton.sub_button.Add(new SinglePicSysphotoButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("PIC_WEIXIN", StringComparison.OrdinalIgnoreCase))
                            {
                                //弹出微信相册发图器
                                subButton.sub_button.Add(new SinglePicWeixinButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else if (subSubButton.type.Equals("SCANCODE_PUSH", StringComparison.OrdinalIgnoreCase))
                            {
                                //扫码推事件
                                subButton.sub_button.Add(new SingleScancodePushButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else
                            {
                                //扫码推事件且弹出“消息接收中”提示框
                                subButton.sub_button.Add(new SingleScancodeWaitmsgButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                        }
                    }
                }

                result = new GetMenuResult()
                {
                    menu = bg
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static WxJsonResult DeleteMenu(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", accessToken);
            var result = Ltchina.Core.Weixin.HttpUtility.Get.GetJson<WxJsonResult>(url);
            return result;
        }
    }
}