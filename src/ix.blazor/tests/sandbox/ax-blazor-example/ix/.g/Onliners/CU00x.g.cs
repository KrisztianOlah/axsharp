using System;
using Ix.Connector;
using Ix.Connector.ValueTypes;
using System.Collections.Generic;

public partial class CU00x : CUBase
{
    public OnlinerString _cuName { get; }

    public CU00x(Ix.Connector.ITwinObject parent, string readableTail, string symbolTail) : base(parent, readableTail, symbolTail + ".$base")
    {
        Symbol = Ix.Connector.Connector.CreateSymbol(parent.Symbol, symbolTail);
        _cuName = @Connector.ConnectorAdapter.AdapterFactory.CreateSTRING(this, "_cuName", "_cuName");
    }

    public async Task<Pocos.CU00x> OnlineToPlainAsync()
    {
        Pocos.CU00x plain = new Pocos.CU00x();
        await this.ReadAsync();
        await base.OnlineToPlainAsync(plain);
        plain._cuName = _cuName.LastValue;
        return plain;
    }

    protected async Task<Pocos.CU00x> OnlineToPlainAsync(Pocos.CU00x plain)
    {
        await base.OnlineToPlainAsync(plain);
        plain._cuName = _cuName.LastValue;
        return plain;
    }

    public async Task<IEnumerable<ITwinPrimitive>> PlainToOnlineAsync(Pocos.CU00x plain)
    {
        await base.PlainToOnlineAsync(plain);
        _cuName.Cyclic = plain._cuName;
        return await this.WriteAsync();
    }
}

public partial class CUBase : Ix.Connector.ITwinObject
{
    public OnlinerString _baseName { get; }

    public CUBase(Ix.Connector.ITwinObject parent, string readableTail, string symbolTail)
    {
        Symbol = Ix.Connector.Connector.CreateSymbol(parent.Symbol, symbolTail);
        this.@SymbolTail = symbolTail;
        this.@Connector = parent.GetConnector();
        this.@Parent = parent;
        HumanReadable = Ix.Connector.Connector.CreateHumanReadable(parent.HumanReadable, readableTail);
        _baseName = @Connector.ConnectorAdapter.AdapterFactory.CreateSTRING(this, "_baseName", "_baseName");
        parent.AddChild(this);
        parent.AddKid(this);
    }

    public async Task<Pocos.CUBase> OnlineToPlainAsync()
    {
        Pocos.CUBase plain = new Pocos.CUBase();
        await this.ReadAsync();
        plain._baseName = _baseName.LastValue;
        return plain;
    }

    protected async Task<Pocos.CUBase> OnlineToPlainAsync(Pocos.CUBase plain)
    {
        plain._baseName = _baseName.LastValue;
        return plain;
    }

    public async Task<IEnumerable<ITwinPrimitive>> PlainToOnlineAsync(Pocos.CUBase plain)
    {
        _baseName.Cyclic = plain._baseName;
        return await this.WriteAsync();
    }

    private IList<Ix.Connector.ITwinObject> Children { get; } = new List<Ix.Connector.ITwinObject>();
    public IEnumerable<Ix.Connector.ITwinObject> GetChildren()
    {
        return Children;
    }

    private IList<Ix.Connector.ITwinElement> Kids { get; } = new List<Ix.Connector.ITwinElement>();
    public IEnumerable<Ix.Connector.ITwinElement> GetKids()
    {
        return Kids;
    }

    private IList<Ix.Connector.ITwinPrimitive> ValueTags { get; } = new List<Ix.Connector.ITwinPrimitive>();
    public IEnumerable<Ix.Connector.ITwinPrimitive> GetValueTags()
    {
        return ValueTags;
    }

    public void AddValueTag(Ix.Connector.ITwinPrimitive valueTag)
    {
        ValueTags.Add(valueTag);
    }

    public void AddKid(Ix.Connector.ITwinElement kid)
    {
        Kids.Add(kid);
    }

    public void AddChild(Ix.Connector.ITwinObject twinObject)
    {
        Children.Add(twinObject);
    }

    protected Ix.Connector.Connector @Connector { get; }

    public Ix.Connector.Connector GetConnector()
    {
        return this.@Connector;
    }

    public string GetSymbolTail()
    {
        return this.SymbolTail;
    }

    public Ix.Connector.ITwinObject GetParent()
    {
        return this.@Parent;
    }

    public string Symbol { get; protected set; }

    public System.String AttributeName { get; set; }

    public string HumanReadable { get; set; }

    protected System.String @SymbolTail { get; set; }

    protected Ix.Connector.ITwinObject @Parent { get; set; }
}