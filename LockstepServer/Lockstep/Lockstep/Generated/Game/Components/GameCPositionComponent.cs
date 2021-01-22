//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Lockstep.CPosition cPosition { get { return (Lockstep.CPosition)GetComponent(GameComponentsLookup.CPosition); } }
    public bool hasCPosition { get { return HasComponent(GameComponentsLookup.CPosition); } }

    public void AddCPosition(System.Numerics.Vector3 newPosition) {
        var index = GameComponentsLookup.CPosition;
        var component = (Lockstep.CPosition)CreateComponent(index, typeof(Lockstep.CPosition));
        component.position = newPosition;
        AddComponent(index, component);
    }

    public void ReplaceCPosition(System.Numerics.Vector3 newPosition) {
        var index = GameComponentsLookup.CPosition;
        var component = (Lockstep.CPosition)CreateComponent(index, typeof(Lockstep.CPosition));
        component.position = newPosition;
        ReplaceComponent(index, component);
    }

    public void RemoveCPosition() {
        RemoveComponent(GameComponentsLookup.CPosition);
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

    static Entitas.IMatcher<GameEntity> _matcherCPosition;

    public static Entitas.IMatcher<GameEntity> CPosition {
        get {
            if (_matcherCPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CPosition);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCPosition = matcher;
            }

            return _matcherCPosition;
        }
    }
}