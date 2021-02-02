//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Lockstep.CPlayerTag cPlayerTagComponent = new Lockstep.CPlayerTag();

    public bool isCPlayerTag {
        get { return HasComponent(GameComponentsLookup.CPlayerTag); }
        set {
            if (value != isCPlayerTag) {
                var index = GameComponentsLookup.CPlayerTag;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : cPlayerTagComponent;

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
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCPlayerTag;

    public static Entitas.IMatcher<GameEntity> CPlayerTag {
        get {
            if (_matcherCPlayerTag == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CPlayerTag);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCPlayerTag = matcher;
            }

            return _matcherCPlayerTag;
        }
    }
}