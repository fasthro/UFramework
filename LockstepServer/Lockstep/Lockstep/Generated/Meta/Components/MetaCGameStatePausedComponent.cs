//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MetaEntity {

    static readonly Lockstep.CGameStatePaused cGameStatePausedComponent = new Lockstep.CGameStatePaused();

    public bool isCGameStatePaused {
        get { return HasComponent(MetaComponentsLookup.CGameStatePaused); }
        set {
            if (value != isCGameStatePaused) {
                var index = MetaComponentsLookup.CGameStatePaused;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : cGameStatePausedComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class MetaMatcher {

    static Entitas.IMatcher<MetaEntity> _matcherCGameStatePaused;

    public static Entitas.IMatcher<MetaEntity> CGameStatePaused {
        get {
            if (_matcherCGameStatePaused == null) {
                var matcher = (Entitas.Matcher<MetaEntity>)Entitas.Matcher<MetaEntity>.AllOf(MetaComponentsLookup.CGameStatePaused);
                matcher.componentNames = MetaComponentsLookup.componentNames;
                _matcherCGameStatePaused = matcher;
            }

            return _matcherCGameStatePaused;
        }
    }
}
