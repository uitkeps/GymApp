using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Classes;
using App.Models.Memberships;
using App.Models.Payments;
using App.Models.Products;

namespace App.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Seeding a  'Administrator' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() },
                new IdentityRole { Id = "6dcabc58-8c5c-4231-9a66-02c038da7429", Name = "Editor", NormalizedName = "Editor".ToUpper() },
                new IdentityRole { Id = "76251a37-7bb0-4f6a-80bd-c454effb7285", Name = "Member", NormalizedName = "Member".ToUpper() }
                );

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@vietgym.com",
                    NormalizedEmail = "admin@vietgym.com",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin"),
                    SecurityStamp = string.Empty,
                    FullName = "Nguyễn Bảo Anh",
                    PhoneNumber = "0866414791",
                    PhoneNumberConfirmed = true,
                    HomeAdress = "Hồ Chí Minh",
                    BirthDate = new DateTime(2002, 9, 29)
                }
            );

            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Title = "Phụ kiện thể thao",
                    Description =
                        "Dần dần mọi người đã ý thức được việc tập thể dục thể thao quan trọng thế nào trong cuộc sống hàng khi Covid đến, ai cũng mong muốn mình " +
                        "có một thể trạng sức khỏe tốt để có thể đấu tranh với bệnh tật vây quanh ta mỗi ngày. Có rất nhiều phương pháp để tập luyện như tập Gym, " +
                        "chạy bộ, bơi lội, đá bóng để có thể rèn luyện sức khỏe của bản thân. Nhưng đi kèm với đó, để có thể tập luyện một cách dễ dàng, hiệu quả " +
                        "và tránh những rủi ro không đáng có trong tập luyện nhất thì chắc chắn bạn không thể thiếu những trợ thủ đắc lực của mình đó chính là những " +
                        "phụ kiện thể thao, tập gym.  Tại thời điểm hiện tại, phụ kiện thể thao tập gym rất đa dạng, có nhiều mẫu mã và mỗi sản phẩm đều chứa một " +
                        "công dụng và lợi ích khác nhau giúp hỗ trợ tập luyện hiệu quả. Cũng chính vì điều này, đặc biệt là với những người mới chơi thể thao đều " +
                        "cân nhắc đắn đo khi lựa chọn các loại phụ kiện thể thao, tập gym phù hợp nhất với mình. Cho nên, Gymstore luôn cố gắng đưa cho bạn những " +
                        "lựa chọn tốt nhất để có thể mua các sản phẩm phụ kiện thể thao, tập gym chính hãng, uy tín giá tốt nhất có thể. ",
                    Slug = "phu-kien-the-thao"
                },
                new Category
                {
                    Id = 2,
                    Title = "Hỗ Trợ Giảm Mỡ",
                    Description =
                        "HỖ TRỢ ĐỐT MỠ là các sản phẩm có công thức mạnh mẽ trong việc tăng khả năng sinh nhiệt của cơ thể, hỗ trợ khả năng đốt cháy chất béo tự " +
                        "nhiên. Với một số chất nổi bật như CLA, L-Carnitine, Yohimbine, Green Tea Extract ",
                    Slug = "ho-tro-giam-mo"
                },
                new Category
                {
                    Id = 3,
                    Title = "Sức Khỏe Toàn Diện",
                    Description = "Sức Khỏe Toàn Diện",
                    Slug = "suc-khoe-toan-dien"
                },
                new Category
                {
                    Id = 4,
                    Title = "Sữa tăng cân",
                    Description =
                        "Sữa Tăng Cân Mass Gainer là dòng sữa tăng cân cho người gầy hỗ trợ bổ sung hàm lượng lớn Calories từ Protein, Carb, Fat, các Vitamin và " +
                        "khoáng chất hỗ trợ cho người gầy tăng cân dễ dàng và hiệu quả. Sữa tăng cân mass gainer có ưu điểm phù hợp với nhiều đối tượng khác nhau, " +
                        "sản phẩm chủ yếu hỗ trợ tăng cân cho người lớn, người tập Gym muốn tăng cân hiệu quả.",
                    Slug = "sua-tang-can"
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, ProductName = "Đai lưng đeo tạ Harbinger PolyPro Dip Belt With Chain", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 850000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "dai-lung-deo-ta-harbinger-polypro-dip-belt-with-chain" },
                new Product { ProductID = 2, ProductName = "Đai lưng Gofit Leather Lifting Belt, 6 Inch", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 800000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "dai-lung-gofit-leather-lifting-belt-6-inch" },
                new Product { ProductID = 3, ProductName = "Harbinger Lifting Grips", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 760000, CategoryID = 3, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "harbinger-lifting-grips" },
                new Product { ProductID = 4, ProductName = "Harbinger 6\" Padded Leather Belt", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 750000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "harbinger-6-padded-leather-belt" },
                new Product { ProductID = 5, ProductName = "Harbinger Training Grip Wristwrap Weightlifting Gloves, Black/Grey", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 710000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "harbinger-training-grip-wristwrap-weightlifting-gloves-blackgrey" },
                new Product { ProductID = 6, ProductName = "Harbinger Flexfit Contour Belt Red, 6 inch", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 700000, CategoryID = 3, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "harbinger-flexfit-contour-belt-red-6-inch" },
                new Product { ProductID = 7, ProductName = "Harbinger 4\" Padded Leather Belt", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 700000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "harbinger-4-padded-leather-belt" },
                new Product { ProductID = 8, ProductName = "Labrada Muscle Mass Gainer 12 Lbs (5443 g)", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 1800000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "labrada-muscle-mass-gainer-12-lbs-5443-g" },
                new Product { ProductID = 9, ProductName = "Applied Critical Mass Original, 6KG (25 Servings)", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 1600000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "applied-critical-mass-original-6kg-25-servings" },
                new Product { ProductID = 10, ProductName = "Kevin Levrone GOLD Lean Mass, 6 KG (30 Servings)", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 1850000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "kevin-levrone-gold-lean-mass-6-kg-30-servings" },
                new Product { ProductID = 11, ProductName = "AP Sports Regimen L-Carnitine 3000mg, 31 Servings", Description = "Đai lưng đeo tạ", Content = "", DateCreated = DateTime.Now, Price = 600000, CategoryID = 2, AuthorId = "8e445865-a24d-4543-a6c6-9443d048cdb9", Slug = "ap-sports-regimen-l-carnitine-3000mg-31-servings" }
            );

            modelBuilder.Entity<ProductPhoto>().HasData(
                new ProductPhoto { Id = 1, ProductID = 1, FileName = "gbcrunmg.webp" },
                new ProductPhoto { Id = 2, ProductID = 2, FileName = "ndhktb2h.webp" },
                new ProductPhoto { Id = 3, ProductID = 2, FileName = "lvbpl5hk.webp" },
                new ProductPhoto { Id = 4, ProductID = 2, FileName = "mzkvjbag.webp" },
                new ProductPhoto { Id = 5, ProductID = 3, FileName = "t3ts2ibq.webp" },
                new ProductPhoto { Id = 6, ProductID = 4, FileName = "wcan2qsl.webp" },
                new ProductPhoto { Id = 7, ProductID = 5, FileName = "0cas3lpe.webp" },
                new ProductPhoto { Id = 8, ProductID = 6, FileName = "nba2br4q.webp" },
                new ProductPhoto { Id = 9, ProductID = 7, FileName = "3dfphy25.webp" },
                new ProductPhoto { Id = 10, ProductID = 8, FileName = "dhlyzh3z.webp" },
                new ProductPhoto { Id = 11, ProductID = 9, FileName = "2kvkadjd.webp" },
                new ProductPhoto { Id = 12, ProductID = 9, FileName = "vmdgv0yz.webp" },
                new ProductPhoto { Id = 13, ProductID = 9, FileName = "odxrwtki.webp" },
                new ProductPhoto { Id = 14, ProductID = 10, FileName = "d2fls0jh.webp" },
                new ProductPhoto { Id = 15, ProductID = 11, FileName = "zuigad2q.webp" },
                new ProductPhoto { Id = 16, ProductID = 11, FileName = "eb3v2agw.webp" },
                new ProductPhoto { Id = 17, ProductID = 11, FileName = "p0ftdyxl.webp" },
                new ProductPhoto { Id = 18, ProductID = 11, FileName = "yimi12em.webp" }
            );

            modelBuilder.Entity<Membership>().HasData(
                new Membership()
                {
                    MembershipId = 4,
                    Level = "Đồng",
                    Fee = 500000,
                    Duration = 3,
                    Hours = 2,
                    Bonus = "Ưu tiên xếp lớp"
                },
                new Membership()
                {
                    MembershipId = 5,
                    Level = "Bạc",
                    Fee = 1000000,
                    Duration = 3,
                    Hours = 3,
                    Bonus = "Được sử dụng phòng tắm, và cá tiện ích gói trên"
                },
                new Membership()
                {
                    MembershipId = 6,
                    Level = "Vàng",
                    Fee = 30000000,
                    Duration = 3,
                    Hours = 4,
                    Bonus = "Có huấn luyện viên cá nhân, và cá tiện ích gói trên"
                }
            );

            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, Name = "NGUYỄN VĂN MƯỜI", DateOfBirth = new DateTime(1969, 04, 02), Gender = "Nữ", Email = "muoinguyen@vietgym.com", Phone = "0909755904", Expertise = "Aerobics", Salary = 15000000 },
                new Instructor { Id = 2, Name = "HUỲNH VĂN BỐN", DateOfBirth = new DateTime(1966, 06, 10), Gender = "Nữ", Email = "bonhuynh@vietgym.com", Phone = "0919686705", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 3, Name = "DƯƠNG THỊ YẾN", DateOfBirth = new DateTime(1968, 04, 04), Gender = "Nữ", Email = "yenduong@vietgym.com", Phone = "0990737806", Expertise = "Yoga", Salary = 15000000 },
                new Instructor { Id = 4, Name = "NGUYỄN THỊ BẢY", DateOfBirth = new DateTime(1972, 10, 19), Gender = "Nữ", Email = "baynguyen@vietgym.com", Phone = "0961708507", Expertise = "Boxing", Salary = 15000000 },
                new Instructor { Id = 5, Name = "NGUYỄN VĂN LÍA", DateOfBirth = new DateTime(1966, 06, 16), Gender = "Nữ", Email = "lianguyen@vietgym.com", Phone = "0981582908", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 6, Name = "TRƯƠNG VĂN ĐÔNG", DateOfBirth = new DateTime(1967, 04, 12), Gender = "Nữ", Email = "dongtruong@vietgym.com", Phone = "0918330410", Expertise = "Yoga", Salary = 15000000 },
                new Instructor { Id = 7, Name = "NGUYỄN ĐẪU", DateOfBirth = new DateTime(1967, 01, 08), Gender = "Nữ", Email = "daunguyen@vietgym.com08", Phone = "0907117111", Expertise = "Boxing", Salary = 15000000 },
                new Instructor { Id = 8, Name = "TRẦN THỊ HÒA", DateOfBirth = new DateTime(1970, 07, 09), Gender = "Nữ", Email = "hoatran@vietgym.com", Phone = "0980976712", Expertise = "Yoga", Salary = 20000000 },
                new Instructor { Id = 9, Name = "TRẦN VIẾT PHỤNG", DateOfBirth = new DateTime(1972, 05, 10), Gender = "Nữ", Email = "phungtran@vietgym.com", Phone = "0919744214", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 10, Name = "LÊ THỊ LỆ HOA", DateOfBirth = new DateTime(1969, 05, 25), Gender = "Nữ", Email = "hoale@vietgym.com", Phone = "0930393915", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 11, Name = "VÕ KIM LINH", DateOfBirth = new DateTime(1967, 03, 04), Gender = "Nữ", Email = "linhvo@vietgym.com", Phone = "0908153717", Expertise = "Aerobics", Salary = 15000000 },
                new Instructor { Id = 12, Name = "LÊ VĂN AN", DateOfBirth = new DateTime(1968, 01, 01), Gender = "Nam", Email = "anle@vietgym.com", Phone = "0934833116", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 13, Name = "NGUYỄN NHƯ PHƯỚC", DateOfBirth = new DateTime(1966, 11, 01), Gender = "Nam", Email = "phuocnguyen@vietgym.com", Phone = "0994750419", Expertise = "Gym", Salary = 20000000 },
                new Instructor { Id = 14, Name = "VÕ MINH HẢI", DateOfBirth = new DateTime(1970, 01, 05), Gender = "Nam", Email = "haivo@vietgym.com", Phone = "0941452818", Expertise = "Yoga", Salary = 15000000 },
                new Instructor { Id = 15, Name = "TRẦN THỊ THU", DateOfBirth = new DateTime(1964, 04, 19), Gender = "Nữ", Email = "thutran@vietgym.com", Phone = "0952726521", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 16, Name = "NGUYỄN VĂN NY", DateOfBirth = new DateTime(1960, 06, 11), Gender = "Nam", Email = "nynguyen@vietgym.com", Phone = "0927438220", Expertise = "Dance", Salary = 10000000 },
                new Instructor { Id = 17, Name = "ĐOÀN VĂN TÂM", DateOfBirth = new DateTime(1967, 06, 27), Gender = "Nam", Email = "tamdoan@vietgym.com", Phone = "0927438220", Expertise = "Boxing", Salary = 15000000 },
                new Instructor { Id = 18, Name = "LÊ THỊ HƯƠNG", DateOfBirth = new DateTime(1967, 04, 10), Gender = "Nữ", Email = "huongle@vietgym.com", Phone = "0925068222", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 19, Name = "HUỲNH THỊ ĐUA", DateOfBirth = new DateTime(1966, 05, 20), Gender = "Nữ", Email = "duahuynh@vietgym.com ", Phone = "0925068222", Expertise = "Boxing", Salary = 10000000 },
                new Instructor { Id = 20, Name = "BÙI THỊ HUYỀN", DateOfBirth = new DateTime(1968, 02, 20), Gender = "Nữ", Email = "huyenbui@vietgym.com", Phone = "0916596324", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 21, Name = "HUỲNH THỊ TUYẾT", DateOfBirth = new DateTime(1968, 01, 01), Gender = "Nữ", Email = "tuyethuynh@vietgym.com", Phone = "0945906825", Expertise = "Dance", Salary = 15000000 },
                new Instructor { Id = 22, Name = "NGUYỄN THỊ PHỤNG", DateOfBirth = new DateTime(1967, 12, 20), Gender = "Nữ", Email = "phungnguyen@vietgym.com", Phone = "0948331326", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 23, Name = "PHẠM THỊ MƯNG", DateOfBirth = new DateTime(1967, 02, 10), Gender = "Nữ", Email = "mungpham@vietgym.com", Phone = "0948331326", Expertise = "Aerobics", Salary = 15000000 },
                new Instructor { Id = 24, Name = "PHÙNG VĂN TÁM", DateOfBirth = new DateTime(1966, 08, 04), Gender = "Nam", Email = "tamphung@vietgym.com", Phone = "0928857527", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 25, Name = "NGUYỄN THỊ XƯA", DateOfBirth = new DateTime(1965, 08, 20), Gender = "Nữ", Email = "xuanguyen@vietgym.com", Phone = "0928857517", Expertise = "Yoga", Salary = 15000000 },
                new Instructor { Id = 26, Name = "NGUYỄN THỊ KIỆP", DateOfBirth = new DateTime(1967, 06, 07), Gender = "Nữ", Email = "kiepnguyen@vietgym.com", Phone = "0912703328", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 27, Name = "UNG THỊ HUỆ", DateOfBirth = new DateTime(1965, 07, 30), Gender = "Nữ", Email = "hueung@vietgym.com", Phone = "0982323328", Expertise = "Boxing", Salary = 15000000 },
                new Instructor { Id = 28, Name = "NGUYỄN THỊ SÁU THỊ SÁU", DateOfBirth = new DateTime(1968, 12, 16), Gender = "Nữ", Email = "saunguyen@vietgym.com", Phone = "0122703328", Expertise = "Gym", Salary = 15000000 },
                new Instructor { Id = 29, Name = "VÕ THỊ KIM HUỆ", DateOfBirth = new DateTime(1965, 06, 26), Gender = "Nữ", Email = "huevo@vietgym.com", Phone = "0948331326", Expertise = "Yoga", Salary = 20000000 }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, RoomName = "A.01", Capacity = 30 },
                new Room { RoomId = 2, RoomName = "A.02", Capacity = 50 },
                new Room { RoomId = 3, RoomName = "A.03", Capacity = 20 },
                new Room { RoomId = 4, RoomName = "A.04", Capacity = 80 },
                new Room { RoomId = 5, RoomName = "A.05", Capacity = 30 },
                new Room { RoomId = 6, RoomName = "A.06", Capacity = 40 },
                new Room { RoomId = 7, RoomName = "A.07", Capacity = 60 },
                new Room { RoomId = 8, RoomName = "A.08", Capacity = 30 },
                new Room { RoomId = 9, RoomName = "A.09", Capacity = 70 },
                new Room { RoomId = 10, RoomName = "B.01", Capacity = 50 },
                new Room { RoomId = 11, RoomName = "B.02", Capacity = 80 },
                new Room { RoomId = 12, RoomName = "B.03", Capacity = 50 },
                new Room { RoomId = 13, RoomName = "B.04", Capacity = 50 },
                new Room { RoomId = 14, RoomName = "B.05", Capacity = 20 },
                new Room { RoomId = 15, RoomName = "B.06", Capacity = 40 },
                new Room { RoomId = 16, RoomName = "B.07", Capacity = 80 },
                new Room { RoomId = 17, RoomName = "B.08", Capacity = 60 },
                new Room { RoomId = 18, RoomName = "B.09", Capacity = 30 },
                new Room { RoomId = 19, RoomName = "C.01", Capacity = 60 },
                new Room { RoomId = 20, RoomName = "C.01", Capacity = 30 }
            );

            modelBuilder.Entity<Class>().HasData(
                new Class { ClassId = 1, ClassTitle = "Gym cho nam giới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 2, PhotoUrl = "b9c58f58-94e1-45b8-a505-f708a9ce0366_blog-9.jpg" },
                new Class { ClassId = 2, ClassTitle = "Gym cho nữ giới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 5, PhotoUrl = "42d4ee61-d335-4972-8f4a-e7b6a92ad6a2_blog-4.jpg" },
                new Class { ClassId = 3, ClassTitle = "Gym cho người mới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 15, PhotoUrl = "31ecfa50-03e9-436a-9417-f2eafde632ab_blog-1.jpg" },
                new Class { ClassId = 4, ClassTitle = "Giảm mỡ bụng", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 3, PhotoUrl = "dc246325-4167-4c1e-869c-9bd8a6825787_blog-5.jpg" },
                new Class { ClassId = 5, ClassTitle = "Giảm cân cấp tốc", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 6, PhotoUrl = "ac083b6e-eaaa-401d-8902-9e12027f0bdd_blog-2.jpg" },
                new Class { ClassId = 6, ClassTitle = "Giảm mỡ toàn thân", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 8, PhotoUrl = "9c18a79d-b12b-41ed-ac5a-96f419b962f6_gallery-1.jpg" },
                new Class { ClassId = 7, ClassTitle = "Tăng chiều cao", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "14783d5f-bfba-493b-8e8a-927f1a20339e_gallery-4.jpg" },
                new Class { ClassId = 8, ClassTitle = "Tăng cường cơ bắp", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "7e6988ea-f5dd-4ef8-8f68-9e799f7e728b_gallery-7.jpg" },
                new Class { ClassId = 9, ClassTitle = "Phục hồi chức năng", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "134c6596-20d7-47e0-961c-b38df4868704_classes-5.jpg" },
                new Class { ClassId = 10, ClassTitle = "Khôi phuc vóc dáng sau sinh", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "041a2478-a9fc-4e3d-aaf6-2b2cd373ef15_classes-2.jpg" },
                new Class { ClassId = 11, ClassTitle = "Aerobic", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "7ce14930-948c-45d2-ae5d-d4440cb14ccf_classes-2.jpg" },
                new Class { ClassId = 12, ClassTitle = "Gym cho nam giới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 20, PhotoUrl = "dd071c8a-49e6-4ca1-8c15-a4c954f56ab6_classes-8.jpg" },
                new Class { ClassId = 13, ClassTitle = "Gym cho nữ giới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 2, PhotoUrl = "7812b275-1d64-4202-bf82-602dd0a64c4f_classes-1.jpg" },
                new Class { ClassId = 14, ClassTitle = "Gym cho người mới", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 5, PhotoUrl = "f825cf9b-6ef5-4db8-b43e-e479f5ffbc49_classes-6.jpg" },
                new Class { ClassId = 15, ClassTitle = "Giảm mỡ bụng", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "41f85a0f-8962-4383-83f6-ad668d0ce8a8_classes-9.jpg" },
                new Class { ClassId = 16, ClassTitle = "Giảm cân cấp tốc", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 6, PhotoUrl = "7b7985a5-9a32-4630-8e2a-783f81b5629d_classes-5.jpg" },
                new Class { ClassId = 17, ClassTitle = "Giảm mỡ toàn thân", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 8, PhotoUrl = "db2f51e3-1d30-4ec4-a173-9ba802d44a56_classes-4.jpg" },
                new Class { ClassId = 18, ClassTitle = "Tăng chiều cao", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "1e4298c8-b920-4c79-9d91-913259651e85_classes-7.jpg" },
                new Class { ClassId = 19, ClassTitle = "Tăng cường cơ bắp", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "3bda819d-aa32-4783-885b-442e768874e5_classes-5.jpg" },
                new Class { ClassId = 20, ClassTitle = "Phục hồi chức năng", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 1, PhotoUrl = "2662208d-fed5-401c-b5fe-89355ece2463_classes-2.jpg" },
                new Class { ClassId = 21, ClassTitle = "Khôi phuc vóc dáng sau sinh", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 3, PhotoUrl = "5fe5ed6b-fa14-405f-ace0-3ef162381282_classes-1.jpg" },
                new Class { ClassId = 22, ClassTitle = "Aerobic", ClassDate = "20/12 - 27/12", ClassPeriod = "10:00 - 11:30", ClassCost = 500000, RoomId = 1, InstructorId = 11, PhotoUrl = "6f4edaf7-2a1d-4bd9-8e53-beca90f2cc58_classes-2.jpg" }
            );

            modelBuilder.Entity<Discount>().HasData(
                new Discount { Id = 1, Code = "Q2QTLHJ401", Percent = 5 },
                new Discount { Id = 2, Code = "Z4VBROL202", Percent = 5 },
                new Discount { Id = 3, Code = "R0MMKGA003", Percent = 5 },
                new Discount { Id = 4, Code = "H6IFRNW004", Percent = 5 },
                new Discount { Id = 5, Code = "M5XHBVT605", Percent = 5 },
                new Discount { Id = 6, Code = "I4OPEJX206", Percent = 5 },
                new Discount { Id = 7, Code = "E5LLKGO207", Percent = 5 },
                new Discount { Id = 8, Code = "P4BMSJG608", Percent = 5 },
                new Discount { Id = 9, Code = "Z9BSUYY809", Percent = 5 },
                new Discount { Id = 10, Code = "S2KGNMD810", Percent = 5 },
                new Discount { Id = 11, Code = "Y4WUAMW811", Percent = 5 },
                new Discount { Id = 12, Code = "I1MWVIM912", Percent = 5 },
                new Discount { Id = 13, Code = "H3WWKLL813", Percent = 5 },
                new Discount { Id = 14, Code = "C7GFORG414", Percent = 5 },
                new Discount { Id = 15, Code = "K5KJVPS315", Percent = 5 },
                new Discount { Id = 16, Code = "U6BROHI716", Percent = 5 },
                new Discount { Id = 17, Code = "X4WFDAR217", Percent = 5 },
                new Discount { Id = 18, Code = "M3NIZFV318", Percent = 5 },
                new Discount { Id = 19, Code = "L4DGUVJ619", Percent = 5 },
                new Discount { Id = 20, Code = "X9XOJTR620", Percent = 5 },
                new Discount { Id = 21, Code = "F4CTPLZ421", Percent = 5 },
                new Discount { Id = 22, Code = "J6VHWGW422", Percent = 5 },
                new Discount { Id = 23, Code = "F6ZYLYU323", Percent = 5 },
                new Discount { Id = 24, Code = "O7WGRLR824", Percent = 5 },
                new Discount { Id = 25, Code = "A1XHZUD525", Percent = 5 },
                new Discount { Id = 26, Code = "L7VXFPZ226", Percent = 5 },
                new Discount { Id = 27, Code = "U5UBXJP727", Percent = 5 },
                new Discount { Id = 28, Code = "Q7RZSJF128", Percent = 5 },
                new Discount { Id = 29, Code = "E9YGYAS029", Percent = 5 },
                new Discount { Id = 30, Code = "M0NHSOR730", Percent = 5 },
                new Discount { Id = 31, Code = "Z5UWPAC931", Percent = 5 },
                new Discount { Id = 32, Code = "M2WNCKA432", Percent = 5 },
                new Discount { Id = 33, Code = "O7LGIUA533", Percent = 5 },
                new Discount { Id = 34, Code = "I3UHOBO634", Percent = 5 },
                new Discount { Id = 35, Code = "P1PKUFX435", Percent = 5 },
                new Discount { Id = 36, Code = "J9HRPEK936", Percent = 5 },
                new Discount { Id = 37, Code = "N2YVKJG837", Percent = 5 },
                new Discount { Id = 38, Code = "C9SJLPI138", Percent = 5 },
                new Discount { Id = 39, Code = "V6UCLLH439", Percent = 5 },
                new Discount { Id = 40, Code = "R3PEANZ640", Percent = 5 },
                new Discount { Id = 41, Code = "D4DHJYY741", Percent = 5 },
                new Discount { Id = 42, Code = "W7OFVKE042", Percent = 5 },
                new Discount { Id = 43, Code = "T7QMCWP843", Percent = 5 },
                new Discount { Id = 44, Code = "R6XCMVL144", Percent = 5 },
                new Discount { Id = 45, Code = "P7NLCSV945", Percent = 5 },
                new Discount { Id = 46, Code = "C6ZLXUW846", Percent = 5 },
                new Discount { Id = 47, Code = "Y4SHNFC447", Percent = 5 },
                new Discount { Id = 48, Code = "N9HOOXK148", Percent = 5 },
                new Discount { Id = 49, Code = "O4NBFXN149", Percent = 5 },
                new Discount { Id = 50, Code = "O5YYVEZ450", Percent = 5 },
                new Discount { Id = 51, Code = "K1HYHTL451", Percent = 10 },
                new Discount { Id = 52, Code = "O9IKYGO052", Percent = 10 },
                new Discount { Id = 53, Code = "J6KFNAP653", Percent = 10 },
                new Discount { Id = 54, Code = "H7TQVVM054", Percent = 10 },
                new Discount { Id = 55, Code = "X6IJXCK655", Percent = 10 },
                new Discount { Id = 56, Code = "S0BJAKQ757", Percent = 10 },
                new Discount { Id = 57, Code = "U1CWCOF258", Percent = 10 },
                new Discount { Id = 58, Code = "W1MFNUV359", Percent = 10 },
                new Discount { Id = 59, Code = "T1CRMIE960", Percent = 10 },
                new Discount { Id = 60, Code = "R3MHXHI061", Percent = 10 },
                new Discount { Id = 61, Code = "C1WRHPY362", Percent = 10 },
                new Discount { Id = 62, Code = "H3SQAKC463", Percent = 10 },
                new Discount { Id = 63, Code = "J4WZPUF964", Percent = 10 },
                new Discount { Id = 64, Code = "D7MJCTV065", Percent = 10 },
                new Discount { Id = 65, Code = "H1JWFNA766", Percent = 10 },
                new Discount { Id = 66, Code = "V1OFGMV967", Percent = 10 },
                new Discount { Id = 67, Code = "H4FDRFU768", Percent = 10 },
                new Discount { Id = 68, Code = "B2NVTYU469", Percent = 10 },
                new Discount { Id = 69, Code = "K9BTMDE770", Percent = 10 },
                new Discount { Id = 70, Code = "T9EAIIA471", Percent = 10 },
                new Discount { Id = 71, Code = "L5OZWIP472", Percent = 10 },
                new Discount { Id = 72, Code = "F7AVIYC073", Percent = 10 },
                new Discount { Id = 73, Code = "P1LYLYU374", Percent = 10 },
                new Discount { Id = 74, Code = "F1RJTIG275", Percent = 10 },
                new Discount { Id = 75, Code = "M0VSVEL376", Percent = 10 },
                new Discount { Id = 76, Code = "T2LPATP477", Percent = 10 },
                new Discount { Id = 77, Code = "O2TCIQP878", Percent = 10 },
                new Discount { Id = 78, Code = "V2OWXLA979", Percent = 10 },
                new Discount { Id = 79, Code = "S7LXPOT080", Percent = 10 },
                new Discount { Id = 80, Code = "Y1YEKDE481", Percent = 10 },
                new Discount { Id = 81, Code = "O1KANNG982", Percent = 10 },
                new Discount { Id = 82, Code = "U3YJGVE883", Percent = 10 },
                new Discount { Id = 83, Code = "K7FSTQB784", Percent = 10 },
                new Discount { Id = 84, Code = "Y0JJMZF485", Percent = 10 },
                new Discount { Id = 85, Code = "X2KLFRO886", Percent = 10 },
                new Discount { Id = 86, Code = "L5NIBJE287", Percent = 10 },
                new Discount { Id = 87, Code = "X8RSTXB888", Percent = 10 },
                new Discount { Id = 88, Code = "R2JTLWV889", Percent = 10 },
                new Discount { Id = 89, Code = "W5SURHV490", Percent = 20 },
                new Discount { Id = 90, Code = "B6AJJHA091", Percent = 20 },
                new Discount { Id = 91, Code = "S0ZFFTD292", Percent = 20 },
                new Discount { Id = 92, Code = "K7CAKCQ193", Percent = 20 },
                new Discount { Id = 93, Code = "X7CQJTD794", Percent = 20 },
                new Discount { Id = 94, Code = "F6SMWTI295", Percent = 20 },
                new Discount { Id = 95, Code = "X1TTIQQ796", Percent = 20 },
                new Discount { Id = 96, Code = "Y7MOOJD997", Percent = 20 },
                new Discount { Id = 97, Code = "D7YIYRO998", Percent = 20 },
                new Discount { Id = 98, Code = "Z5FUNZS799", Percent = 20 },
                new Discount { Id = 99, Code = "O4MAHTL356", Percent = 20 }
            );
        }
    }
}