using System;

namespace Pocos
{
    public partial class integrated
    {
        public MonsterData.Monster Monster { get; set; } = new MonsterData.Monster();
        public MonsterData.Monster OnlineToPlain_should_copy_entire_structure { get; set; } = new MonsterData.Monster();
        public MonsterData.Monster PlainToOnline_should_copy_entire_structure { get; set; } = new MonsterData.Monster();
        public MonsterData.Monster OnlineToShadowAsync_should_copy_entire_structure { get; set; } = new MonsterData.Monster();
        public MonsterData.Monster ShadowToOnlineAsync_should_copy_entire_structure { get; set; } = new MonsterData.Monster();
        public Pokus Pokus { get; set; } = new Pokus();
    }

    public partial class Pokus
    {
        public Nested Nested { get; set; } = new Nested();
    }

    public partial class Nested
    {
        public string SomeString { get; set; } = string.Empty;
        public Int16 SomeInt { get; set; }

        public Byte SomeByte { get; set; }
    }
}