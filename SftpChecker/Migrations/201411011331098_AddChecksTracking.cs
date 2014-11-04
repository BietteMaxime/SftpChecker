namespace SftpChecker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChecksTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Process = c.String(maxLength: 4000),
                        Timestamp = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        Message = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Checks");
        }
    }
}
