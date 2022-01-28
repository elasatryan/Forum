namespace Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AspNetUsers", "UpdatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AspNetUsers", "DeletedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Country", c => c.String(maxLength: 50));

        }
        
        public override void Down()
        {
        }
    }
}
