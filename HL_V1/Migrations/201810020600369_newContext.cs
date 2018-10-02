namespace HL_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleApproveViewModels",
                c => new
                    {
                        ArticleID = c.Guid(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        AuthorID = c.String(),
                        Category = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        ArticleStatus = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ArticleID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArticleApproveViewModels");
        }
    }
}
