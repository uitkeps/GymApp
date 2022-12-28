using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gymapp.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expertise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    HomeAdress = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    InstructorId = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignupClasses",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SignupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignupClasses", x => new { x.ClassId, x.UserId, x.PaymentId });
                    table.ForeignKey(
                        name: "FK_SignupClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignupClasses_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignupClasses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignupMemberships",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SignupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignupMemberships", x => new { x.MembershipId, x.UserId, x.PaymentId });
                    table.ForeignKey(
                        name: "FK_SignupMemberships_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignupMemberships_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignupMemberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => new { x.PaymentID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Payments_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPhoto_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Description", "Slug", "Title" },
                values: new object[,]
                {
                    { 1, "Dần dần mọi người đã ý thức được việc tập thể dục thể thao quan trọng thế nào trong cuộc sống hàng khi Covid đến, ai cũng mong muốn mình có một thể trạng sức khỏe tốt để có thể đấu tranh với bệnh tật vây quanh ta mỗi ngày. Có rất nhiều phương pháp để tập luyện như tập Gym, chạy bộ, bơi lội, đá bóng để có thể rèn luyện sức khỏe của bản thân. Nhưng đi kèm với đó, để có thể tập luyện một cách dễ dàng, hiệu quả và tránh những rủi ro không đáng có trong tập luyện nhất thì chắc chắn bạn không thể thiếu những trợ thủ đắc lực của mình đó chính là những phụ kiện thể thao, tập gym.  Tại thời điểm hiện tại, phụ kiện thể thao tập gym rất đa dạng, có nhiều mẫu mã và mỗi sản phẩm đều chứa một công dụng và lợi ích khác nhau giúp hỗ trợ tập luyện hiệu quả. Cũng chính vì điều này, đặc biệt là với những người mới chơi thể thao đều cân nhắc đắn đo khi lựa chọn các loại phụ kiện thể thao, tập gym phù hợp nhất với mình. Cho nên, Gymstore luôn cố gắng đưa cho bạn những lựa chọn tốt nhất để có thể mua các sản phẩm phụ kiện thể thao, tập gym chính hãng, uy tín giá tốt nhất có thể. ", "phu-kien-the-thao", "Phụ kiện thể thao" },
                    { 2, "HỖ TRỢ ĐỐT MỠ là các sản phẩm có công thức mạnh mẽ trong việc tăng khả năng sinh nhiệt của cơ thể, hỗ trợ khả năng đốt cháy chất béo tự nhiên. Với một số chất nổi bật như CLA, L-Carnitine, Yohimbine, Green Tea Extract ", "ho-tro-giam-mo", "Hỗ Trợ Giảm Mỡ" },
                    { 3, "Sức Khỏe Toàn Diện", "suc-khoe-toan-dien", "Sức Khỏe Toàn Diện" },
                    { 4, "Sữa Tăng Cân Mass Gainer là dòng sữa tăng cân cho người gầy hỗ trợ bổ sung hàm lượng lớn Calories từ Protein, Carb, Fat, các Vitamin và khoáng chất hỗ trợ cho người gầy tăng cân dễ dàng và hiệu quả. Sữa tăng cân mass gainer có ưu điểm phù hợp với nhiều đối tượng khác nhau, sản phẩm chủ yếu hỗ trợ tăng cân cho người lớn, người tập Gym muốn tăng cân hiệu quả.", "sua-tang-can", "Sữa tăng cân" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Code", "Percent" },
                values: new object[,]
                {
                    { 1, "Q2QTLHJ401", 5 },
                    { 2, "Z4VBROL202", 5 },
                    { 3, "R0MMKGA003", 5 },
                    { 4, "H6IFRNW004", 5 },
                    { 5, "M5XHBVT605", 5 },
                    { 6, "I4OPEJX206", 5 },
                    { 7, "E5LLKGO207", 5 },
                    { 8, "P4BMSJG608", 5 },
                    { 9, "Z9BSUYY809", 5 },
                    { 10, "S2KGNMD810", 5 },
                    { 11, "Y4WUAMW811", 5 },
                    { 12, "I1MWVIM912", 5 },
                    { 13, "H3WWKLL813", 5 },
                    { 14, "C7GFORG414", 5 },
                    { 15, "K5KJVPS315", 5 },
                    { 16, "U6BROHI716", 5 },
                    { 17, "X4WFDAR217", 5 },
                    { 18, "M3NIZFV318", 5 },
                    { 19, "L4DGUVJ619", 5 },
                    { 20, "X9XOJTR620", 5 },
                    { 21, "F4CTPLZ421", 5 },
                    { 22, "J6VHWGW422", 5 },
                    { 23, "F6ZYLYU323", 5 },
                    { 24, "O7WGRLR824", 5 },
                    { 25, "A1XHZUD525", 5 },
                    { 26, "L7VXFPZ226", 5 },
                    { 27, "U5UBXJP727", 5 },
                    { 28, "Q7RZSJF128", 5 },
                    { 29, "E9YGYAS029", 5 },
                    { 30, "M0NHSOR730", 5 },
                    { 31, "Z5UWPAC931", 5 },
                    { 32, "M2WNCKA432", 5 },
                    { 33, "O7LGIUA533", 5 },
                    { 34, "I3UHOBO634", 5 },
                    { 35, "P1PKUFX435", 5 },
                    { 36, "J9HRPEK936", 5 },
                    { 37, "N2YVKJG837", 5 },
                    { 38, "C9SJLPI138", 5 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Code", "Percent" },
                values: new object[,]
                {
                    { 39, "V6UCLLH439", 5 },
                    { 40, "R3PEANZ640", 5 },
                    { 41, "D4DHJYY741", 5 },
                    { 42, "W7OFVKE042", 5 },
                    { 43, "T7QMCWP843", 5 },
                    { 44, "R6XCMVL144", 5 },
                    { 45, "P7NLCSV945", 5 },
                    { 46, "C6ZLXUW846", 5 },
                    { 47, "Y4SHNFC447", 5 },
                    { 48, "N9HOOXK148", 5 },
                    { 49, "O4NBFXN149", 5 },
                    { 50, "O5YYVEZ450", 5 },
                    { 51, "K1HYHTL451", 10 },
                    { 52, "O9IKYGO052", 10 },
                    { 53, "J6KFNAP653", 10 },
                    { 54, "H7TQVVM054", 10 },
                    { 55, "X6IJXCK655", 10 },
                    { 56, "S0BJAKQ757", 10 },
                    { 57, "U1CWCOF258", 10 },
                    { 58, "W1MFNUV359", 10 },
                    { 59, "T1CRMIE960", 10 },
                    { 60, "R3MHXHI061", 10 },
                    { 61, "C1WRHPY362", 10 },
                    { 62, "H3SQAKC463", 10 },
                    { 63, "J4WZPUF964", 10 },
                    { 64, "D7MJCTV065", 10 },
                    { 65, "H1JWFNA766", 10 },
                    { 66, "V1OFGMV967", 10 },
                    { 67, "H4FDRFU768", 10 },
                    { 68, "B2NVTYU469", 10 },
                    { 69, "K9BTMDE770", 10 },
                    { 70, "T9EAIIA471", 10 },
                    { 71, "L5OZWIP472", 10 },
                    { 72, "F7AVIYC073", 10 },
                    { 73, "P1LYLYU374", 10 },
                    { 74, "F1RJTIG275", 10 },
                    { 75, "M0VSVEL376", 10 },
                    { 76, "T2LPATP477", 10 },
                    { 77, "O2TCIQP878", 10 },
                    { 78, "V2OWXLA979", 10 },
                    { 79, "S7LXPOT080", 10 },
                    { 80, "Y1YEKDE481", 10 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Code", "Percent" },
                values: new object[,]
                {
                    { 81, "O1KANNG982", 10 },
                    { 82, "U3YJGVE883", 10 },
                    { 83, "K7FSTQB784", 10 },
                    { 84, "Y0JJMZF485", 10 },
                    { 85, "X2KLFRO886", 10 },
                    { 86, "L5NIBJE287", 10 },
                    { 87, "X8RSTXB888", 10 },
                    { 88, "R2JTLWV889", 10 },
                    { 89, "W5SURHV490", 20 },
                    { 90, "B6AJJHA091", 20 },
                    { 91, "S0ZFFTD292", 20 },
                    { 92, "K7CAKCQ193", 20 },
                    { 93, "X7CQJTD794", 20 },
                    { 94, "F6SMWTI295", 20 },
                    { 95, "X1TTIQQ796", 20 },
                    { 96, "Y7MOOJD997", 20 },
                    { 97, "D7YIYRO998", 20 },
                    { 98, "Z5FUNZS799", 20 },
                    { 99, "O4MAHTL356", 20 }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "DateOfBirth", "Email", "Expertise", "Gender", "Name", "Phone", "Salary" },
                values: new object[,]
                {
                    { 1, new DateTime(1969, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "muoinguyen@vietgym.com", "Aerobics", "Nữ", "NGUYỄN VĂN MƯỜI", "0909755904", 15000000m },
                    { 2, new DateTime(1966, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "bonhuynh@vietgym.com", "Gym", "Nữ", "HUỲNH VĂN BỐN", "0919686705", 15000000m },
                    { 3, new DateTime(1968, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "yenduong@vietgym.com", "Yoga", "Nữ", "DƯƠNG THỊ YẾN", "0990737806", 15000000m },
                    { 4, new DateTime(1972, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "baynguyen@vietgym.com", "Boxing", "Nữ", "NGUYỄN THỊ BẢY", "0961708507", 15000000m },
                    { 5, new DateTime(1966, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "lianguyen@vietgym.com", "Gym", "Nữ", "NGUYỄN VĂN LÍA", "0981582908", 15000000m },
                    { 6, new DateTime(1967, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "dongtruong@vietgym.com", "Yoga", "Nữ", "TRƯƠNG VĂN ĐÔNG", "0918330410", 15000000m },
                    { 7, new DateTime(1967, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "daunguyen@vietgym.com08", "Boxing", "Nữ", "NGUYỄN ĐẪU", "0907117111", 15000000m },
                    { 8, new DateTime(1970, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoatran@vietgym.com", "Yoga", "Nữ", "TRẦN THỊ HÒA", "0980976712", 20000000m },
                    { 9, new DateTime(1972, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "phungtran@vietgym.com", "Gym", "Nữ", "TRẦN VIẾT PHỤNG", "0919744214", 15000000m },
                    { 10, new DateTime(1969, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoale@vietgym.com", "Gym", "Nữ", "LÊ THỊ LỆ HOA", "0930393915", 15000000m },
                    { 11, new DateTime(1967, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "linhvo@vietgym.com", "Aerobics", "Nữ", "VÕ KIM LINH", "0908153717", 15000000m },
                    { 12, new DateTime(1968, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "anle@vietgym.com", "Gym", "Nam", "LÊ VĂN AN", "0934833116", 15000000m },
                    { 13, new DateTime(1966, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "phuocnguyen@vietgym.com", "Gym", "Nam", "NGUYỄN NHƯ PHƯỚC", "0994750419", 20000000m },
                    { 14, new DateTime(1970, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "haivo@vietgym.com", "Yoga", "Nam", "VÕ MINH HẢI", "0941452818", 15000000m },
                    { 15, new DateTime(1964, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "thutran@vietgym.com", "Gym", "Nữ", "TRẦN THỊ THU", "0952726521", 15000000m },
                    { 16, new DateTime(1960, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "nynguyen@vietgym.com", "Dance", "Nam", "NGUYỄN VĂN NY", "0927438220", 10000000m },
                    { 17, new DateTime(1967, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "tamdoan@vietgym.com", "Boxing", "Nam", "ĐOÀN VĂN TÂM", "0927438220", 15000000m },
                    { 18, new DateTime(1967, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "huongle@vietgym.com", "Gym", "Nữ", "LÊ THỊ HƯƠNG", "0925068222", 15000000m },
                    { 19, new DateTime(1966, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "duahuynh@vietgym.com ", "Boxing", "Nữ", "HUỲNH THỊ ĐUA", "0925068222", 10000000m },
                    { 20, new DateTime(1968, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "huyenbui@vietgym.com", "Gym", "Nữ", "BÙI THỊ HUYỀN", "0916596324", 15000000m },
                    { 21, new DateTime(1968, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tuyethuynh@vietgym.com", "Dance", "Nữ", "HUỲNH THỊ TUYẾT", "0945906825", 15000000m },
                    { 22, new DateTime(1967, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "phungnguyen@vietgym.com", "Gym", "Nữ", "NGUYỄN THỊ PHỤNG", "0948331326", 15000000m },
                    { 23, new DateTime(1967, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mungpham@vietgym.com", "Aerobics", "Nữ", "PHẠM THỊ MƯNG", "0948331326", 15000000m }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "DateOfBirth", "Email", "Expertise", "Gender", "Name", "Phone", "Salary" },
                values: new object[,]
                {
                    { 24, new DateTime(1966, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "tamphung@vietgym.com", "Gym", "Nam", "PHÙNG VĂN TÁM", "0928857527", 15000000m },
                    { 25, new DateTime(1965, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "xuanguyen@vietgym.com", "Yoga", "Nữ", "NGUYỄN THỊ XƯA", "0928857517", 15000000m },
                    { 26, new DateTime(1967, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "kiepnguyen@vietgym.com", "Gym", "Nữ", "NGUYỄN THỊ KIỆP", "0912703328", 15000000m },
                    { 27, new DateTime(1965, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "hueung@vietgym.com", "Boxing", "Nữ", "UNG THỊ HUỆ", "0982323328", 15000000m },
                    { 28, new DateTime(1968, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "saunguyen@vietgym.com", "Gym", "Nữ", "NGUYỄN THỊ SÁU THỊ SÁU", "0122703328", 15000000m },
                    { 29, new DateTime(1965, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "huevo@vietgym.com", "Yoga", "Nữ", "VÕ THỊ KIM HUỆ", "0948331326", 20000000m }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipId", "Bonus", "Duration", "Fee", "Hours", "Level" },
                values: new object[,]
                {
                    { 4, "Ưu tiên xếp lớp", 3, 500000m, 2, "Đồng" },
                    { 5, "Được sử dụng phòng tắm, và cá tiện ích gói trên", 3, 1000000m, 3, "Bạc" },
                    { 6, "Có huấn luyện viên cá nhân, và cá tiện ích gói trên", 3, 30000000m, 4, "Vàng" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", "9cc53335-b1c7-4c0b-8aa1-3811bbb0f6c0", "Administrator", "ADMINISTRATOR" },
                    { "6dcabc58-8c5c-4231-9a66-02c038da7429", "3a44dc59-d93e-4c19-ac11-1fa4da7c6db8", "Editor", "EDITOR" },
                    { "76251a37-7bb0-4f6a-80bd-c454effb7285", "7a9e1575-bf6e-4ee1-87df-442c4aa5bee0", "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Capacity", "RoomName" },
                values: new object[,]
                {
                    { 1, 30, "A.01" },
                    { 2, 50, "A.02" },
                    { 3, 20, "A.03" },
                    { 4, 80, "A.04" },
                    { 5, 30, "A.05" },
                    { 6, 40, "A.06" },
                    { 7, 60, "A.07" },
                    { 8, 30, "A.08" },
                    { 9, 70, "A.09" },
                    { 10, 50, "B.01" },
                    { 11, 80, "B.02" },
                    { 12, 50, "B.03" },
                    { 13, 50, "B.04" },
                    { 14, 20, "B.05" },
                    { 15, 40, "B.06" },
                    { 16, 80, "B.07" },
                    { 17, 60, "B.08" },
                    { 18, 30, "B.09" },
                    { 19, 60, "C.01" },
                    { 20, 30, "C.01" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "HomeAdress", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, new DateTime(2002, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "61181b81-daca-4589-a48d-fff2ac72fa8c", "admin@vietgym.com", true, "Nguyễn Bảo Anh", "Hồ Chí Minh", false, null, "admin@vietgym.com", "ADMIN", "AQAAAAEAACcQAAAAEFO+xqSBx6b5A5TtizV2qm+mFrtL9petZQmTY6LvLTiQmX0fxBmtdRnmL5cmzdxQUg==", "0866414791", true, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "ClassCost", "ClassDate", "ClassPeriod", "ClassTitle", "InstructorId", "PhotoUrl", "RoomId" },
                values: new object[,]
                {
                    { 1, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho nam giới", 2, "b9c58f58-94e1-45b8-a505-f708a9ce0366_blog-9.jpg", 1 },
                    { 2, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho nữ giới", 5, "42d4ee61-d335-4972-8f4a-e7b6a92ad6a2_blog-4.jpg", 1 },
                    { 3, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho người mới", 15, "31ecfa50-03e9-436a-9417-f2eafde632ab_blog-1.jpg", 1 },
                    { 4, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm mỡ bụng", 3, "dc246325-4167-4c1e-869c-9bd8a6825787_blog-5.jpg", 1 },
                    { 5, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm cân cấp tốc", 6, "ac083b6e-eaaa-401d-8902-9e12027f0bdd_blog-2.jpg", 1 },
                    { 6, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm mỡ toàn thân", 8, "9c18a79d-b12b-41ed-ac5a-96f419b962f6_gallery-1.jpg", 1 },
                    { 7, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Tăng chiều cao", 1, "14783d5f-bfba-493b-8e8a-927f1a20339e_gallery-4.jpg", 1 },
                    { 8, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Tăng cường cơ bắp", 1, "7e6988ea-f5dd-4ef8-8f68-9e799f7e728b_gallery-7.jpg", 1 },
                    { 9, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Phục hồi chức năng", 1, "134c6596-20d7-47e0-961c-b38df4868704_classes-5.jpg", 1 },
                    { 10, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Khôi phuc vóc dáng sau sinh", 1, "041a2478-a9fc-4e3d-aaf6-2b2cd373ef15_classes-2.jpg", 1 },
                    { 11, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Aerobic", 1, "7ce14930-948c-45d2-ae5d-d4440cb14ccf_classes-2.jpg", 1 },
                    { 12, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho nam giới", 20, "dd071c8a-49e6-4ca1-8c15-a4c954f56ab6_classes-8.jpg", 1 },
                    { 13, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho nữ giới", 2, "7812b275-1d64-4202-bf82-602dd0a64c4f_classes-1.jpg", 1 },
                    { 14, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Gym cho người mới", 5, "f825cf9b-6ef5-4db8-b43e-e479f5ffbc49_classes-6.jpg", 1 },
                    { 15, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm mỡ bụng", 1, "41f85a0f-8962-4383-83f6-ad668d0ce8a8_classes-9.jpg", 1 },
                    { 16, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm cân cấp tốc", 6, "7b7985a5-9a32-4630-8e2a-783f81b5629d_classes-5.jpg", 1 },
                    { 17, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Giảm mỡ toàn thân", 8, "db2f51e3-1d30-4ec4-a173-9ba802d44a56_classes-4.jpg", 1 },
                    { 18, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Tăng chiều cao", 1, "1e4298c8-b920-4c79-9d91-913259651e85_classes-7.jpg", 1 },
                    { 19, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Tăng cường cơ bắp", 1, "3bda819d-aa32-4783-885b-442e768874e5_classes-5.jpg", 1 },
                    { 20, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Phục hồi chức năng", 1, "2662208d-fed5-401c-b5fe-89355ece2463_classes-2.jpg", 1 },
                    { 21, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Khôi phuc vóc dáng sau sinh", 3, "5fe5ed6b-fa14-405f-ace0-3ef162381282_classes-1.jpg", 1 },
                    { 22, 500000m, "20/12 - 27/12", "10:00 - 11:30", "Aerobic", 11, "6f4edaf7-2a1d-4bd9-8e53-beca90f2cc58_classes-2.jpg", 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "AuthorId", "CategoryID", "Content", "DateCreated", "DateUpdated", "Description", "Price", "ProductName", "Slug" },
                values: new object[,]
                {
                    { 1, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6453), null, "Đai lưng đeo tạ", 850000m, "Đai lưng đeo tạ Harbinger PolyPro Dip Belt With Chain", "dai-lung-deo-ta-harbinger-polypro-dip-belt-with-chain" },
                    { 2, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6463), null, "Đai lưng đeo tạ", 800000m, "Đai lưng Gofit Leather Lifting Belt, 6 Inch", "dai-lung-gofit-leather-lifting-belt-6-inch" },
                    { 3, "8e445865-a24d-4543-a6c6-9443d048cdb9", 3, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6465), null, "Đai lưng đeo tạ", 760000m, "Harbinger Lifting Grips", "harbinger-lifting-grips" },
                    { 4, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6466), null, "Đai lưng đeo tạ", 750000m, "Harbinger 6\" Padded Leather Belt", "harbinger-6-padded-leather-belt" },
                    { 5, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6468), null, "Đai lưng đeo tạ", 710000m, "Harbinger Training Grip Wristwrap Weightlifting Gloves, Black/Grey", "harbinger-training-grip-wristwrap-weightlifting-gloves-blackgrey" },
                    { 6, "8e445865-a24d-4543-a6c6-9443d048cdb9", 3, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6469), null, "Đai lưng đeo tạ", 700000m, "Harbinger Flexfit Contour Belt Red, 6 inch", "harbinger-flexfit-contour-belt-red-6-inch" },
                    { 7, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6471), null, "Đai lưng đeo tạ", 700000m, "Harbinger 4\" Padded Leather Belt", "harbinger-4-padded-leather-belt" },
                    { 8, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6472), null, "Đai lưng đeo tạ", 1800000m, "Labrada Muscle Mass Gainer 12 Lbs (5443 g)", "labrada-muscle-mass-gainer-12-lbs-5443-g" },
                    { 9, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6473), null, "Đai lưng đeo tạ", 1600000m, "Applied Critical Mass Original, 6KG (25 Servings)", "applied-critical-mass-original-6kg-25-servings" },
                    { 10, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6475), null, "Đai lưng đeo tạ", 1850000m, "Kevin Levrone GOLD Lean Mass, 6 KG (30 Servings)", "kevin-levrone-gold-lean-mass-6-kg-30-servings" },
                    { 11, "8e445865-a24d-4543-a6c6-9443d048cdb9", 2, "", new DateTime(2022, 12, 22, 8, 53, 0, 408, DateTimeKind.Local).AddTicks(6476), null, "Đai lưng đeo tạ", 600000m, "AP Sports Regimen L-Carnitine 3000mg, 31 Servings", "ap-sports-regimen-l-carnitine-3000mg-31-servings" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.InsertData(
                table: "ProductPhoto",
                columns: new[] { "Id", "FileName", "ProductID" },
                values: new object[,]
                {
                    { 1, "gbcrunmg.webp", 1 },
                    { 2, "ndhktb2h.webp", 2 },
                    { 3, "lvbpl5hk.webp", 2 },
                    { 4, "mzkvjbag.webp", 2 },
                    { 5, "t3ts2ibq.webp", 3 },
                    { 6, "wcan2qsl.webp", 4 },
                    { 7, "0cas3lpe.webp", 5 },
                    { 8, "nba2br4q.webp", 6 },
                    { 9, "3dfphy25.webp", 7 },
                    { 10, "dhlyzh3z.webp", 8 },
                    { 11, "2kvkadjd.webp", 9 },
                    { 12, "vmdgv0yz.webp", 9 },
                    { 13, "odxrwtki.webp", 9 },
                    { 14, "d2fls0jh.webp", 10 },
                    { 15, "zuigad2q.webp", 11 },
                    { 16, "eb3v2agw.webp", 11 },
                    { 17, "p0ftdyxl.webp", 11 },
                    { 18, "yimi12em.webp", 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_InstructorId",
                table: "Classes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_RoomId",
                table: "Classes",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_ProductID",
                table: "PaymentDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DiscountId",
                table: "Payments",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserID",
                table: "Payments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhoto_ProductID",
                table: "ProductPhoto",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuthorId",
                table: "Products",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SignupClasses_PaymentId",
                table: "SignupClasses",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SignupClasses_UserId",
                table: "SignupClasses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SignupMemberships_PaymentId",
                table: "SignupMemberships",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SignupMemberships_UserId",
                table: "SignupMemberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "ProductPhoto");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SignupClasses");

            migrationBuilder.DropTable(
                name: "SignupMemberships");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
