namespace DataBasesAndModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorMigration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Surname", c => c.String(nullable: false, maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "Surname", c => c.String());
        }
    }
}
