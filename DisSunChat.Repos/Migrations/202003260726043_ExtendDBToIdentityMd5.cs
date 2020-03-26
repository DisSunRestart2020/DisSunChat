namespace DisSunChat.Repos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendDBToIdentityMd5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatHistories", "IdentityMd5", c => c.String(maxLength: 50));
            AddColumn("dbo.ChatHistories", "ImgIndex", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatHistories", "ImgIndex");
            DropColumn("dbo.ChatHistories", "IdentityMd5");
        }
    }
}
