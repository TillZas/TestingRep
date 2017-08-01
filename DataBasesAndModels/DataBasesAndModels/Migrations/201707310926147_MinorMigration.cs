namespace DataBasesAndModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false, maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "Name", c => c.String());
        }
    }
}
