namespace DisSunChat.Repos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatHistories",
                c => new
                    {
                        HID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(maxLength: 200),
                        CreateTime = c.DateTime(nullable: false),
                        ChatContent = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.HID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChatHistories");
        }
    }
}
