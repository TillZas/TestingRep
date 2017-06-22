namespace DataBasesAndModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TownMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Streets", "CreationAge", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Streets", "CreationAge");
        }
    }
}
