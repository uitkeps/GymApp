using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace App.Menu
{
    public class AdminSidebarService
    {
        private readonly IUrlHelper UrlHelper;
        public List<SidebarItem> Items { get; set; } = new List<SidebarItem>();

        public AdminSidebarService(IUrlHelperFactory factory, IActionContextAccessor action)
        {
            UrlHelper = factory.GetUrlHelper(action.ActionContext);
            // Khoi tao cac muc sidebar

            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Heading, Title = "Quản lý chung" });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "DbManage",
                Action = "Index",
                Area = "Database",
                Title = "Quản lý Database",
                AwesomeIcon = "fas fa-database"
            });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "Contact",
                Action = "Index",
                Area = "Contact",
                Title = "Quản lý liên hệ",
                AwesomeIcon = "far fa-address-card"
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Phân quyền & thành viên",
                AwesomeIcon = "far fa-folder",
                collapseID = "role",
                Items = new List<SidebarItem>() {
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Index",
                                Area = "Identity",
                                Title = "Các vai trò (role)"
                        },
                         new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Create",
                                Area = "Identity",
                                Title = "Tạo role mới"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "User",
                                Action = "Index",
                                Area = "Identity",
                                Title = "Danh sách thành viên"
                        },
                    },
            });

            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý sản phẩm",
                AwesomeIcon = "fa-solid fa-dumbbell",
                collapseID = "product",
                Items = new List<SidebarItem>() {
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Index",
                                Area = "Product",
                                Title = "Các danh mục sản phẩm"
                        },
                         new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Create",
                                Area = "Product",
                                Title = "Tạo danh mục sản phẩm"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Product",
                                Action = "Index",
                                Area = "Product",
                                Title = "Các sản phẩm"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Product",
                                Action = "Create",
                                Area = "Product",
                                Title = "Tạo sản phẩm"
                        },
                    },
            });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý huấn luyện viên",
                AwesomeIcon = "fa-solid fa-user",
                collapseID = "instructor",
                Items = new List<SidebarItem>() {
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Instructor",
                        Action = "Index",
                        Area = "Class",
                        Title = "Danh sách huấn luyện viên"
                    },
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Instructor",
                        Action = "Create",
                        Area = "Class",
                        Title = "Tạo khóa tập mới"
                    },
                },
            });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý phòng tập",
                AwesomeIcon = "fa-solid fa-house",
                collapseID = "room",
                Items = new List<SidebarItem>() {
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Room",
                        Action = "Index",
                        Area = "Class",
                        Title = "Danh sách phòng tập"
                    },
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Room",
                        Action = "Create",
                        Area = "Class",
                        Title = "Tạo phòng tập mới"
                    },
                },
            });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý khóa tập",
                AwesomeIcon = "far fa-folder",
                collapseID = "class",
                Items = new List<SidebarItem>() {
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Class",
                        Action = "Index",
                        Area = "Class",
                        Title = "Danh sách khóa tập"
                    },
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Class",
                        Action = "Create",
                        Area = "Class",
                        Title = "Tạo khóa tập mới"
                    },
                },
            });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý gói tập",
                AwesomeIcon = "far fa-folder",
                collapseID = "membership",
                Items = new List<SidebarItem>() {
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Membership",
                        Action = "Index",
                        Area = "Membership",
                        Title = "Danh sách gói tập"
                    },
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Membership",
                        Action = "Create",
                        Area = "Membership",
                        Title = "Tạo gói tập mới"
                    },
                },
            });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý mã giảm giá",
                AwesomeIcon = "fa-solid fa-sack-dollar",
                collapseID = "discount",
                Items = new List<SidebarItem>() {
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Discount",
                        Action = "Index",
                        Area = "Product",
                        Title = "Danh sách mã giảm giá"
                    },
                    new SidebarItem() {
                        Type = SidebarItemType.NavItem,
                        Controller = "Discount",
                        Action = "Create",
                        Area = "Product",
                        Title = "Tạo mã giảm giá mới"
                    },
                },
            });
        }

        public string renderHtml()
        {
            var html = new StringBuilder();

            foreach (var item in Items)
            {
                html.Append(item.RenderHtml(UrlHelper));
            }

            return html.ToString();
        }

        public void SetActive(string Controller, string Action, string Area)
        {
            foreach (var item in Items)
            {
                if (item.Controller == Controller && item.Action == Action && item.Area == Area)
                {
                    item.IsActive = true;
                    return;
                }
                else
                {
                    if (item.Items != null)
                    {
                        foreach (var childItem in item.Items)
                        {
                            if (childItem.Controller == Controller && childItem.Action == Action && childItem.Area == Area)
                            {
                                childItem.IsActive = true;
                                item.IsActive = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}