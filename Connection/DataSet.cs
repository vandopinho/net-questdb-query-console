namespace QuestDbQueryConsole.Connection
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using QuestDbQueryConsole.Entity;

    public partial class DataSet : DbContext
    {
        public DataSet()
            : base("name=DataTeste")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<DadosEntity> DadosEntity { get; set; }
    }
}