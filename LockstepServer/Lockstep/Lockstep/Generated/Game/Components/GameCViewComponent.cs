//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Lockstep.CView cView { get { return (Lockstep.CView)GetComponent(GameComponentsLookup.CView); } }
    public bool hasCView { get { return HasComponent(GameComponentsLookup.CView); } }

    public void AddCView(Lockstep.IView newView) {
        var index = GameComponentsLookup.CView;
        var component = (Lockstep.CView)CreateComponent(index, typeof(Lockstep.CView));
        component.view = newView;
        AddComponent(index, component);
    }

    public void ReplaceCView(Lockstep.IView newView) {
        var index = GameComponentsLookup.CView;
        var component = (Lockstep.CView)CreateComponent(index, typeof(Lockstep.CView));
        component.view = newView;
        ReplaceComponent(index, component);
    }

    public void RemoveCView() {
        RemoveComponent(GameComponentsLookup.CView);
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

    static Entitas.IMatcher<GameEntity> _matcherCView;

    public static Entitas.IMatcher<GameEntity> CView {
        get {
            if (_matcherCView == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CView);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCView = matcher;
            }

            return _matcherCView;
        }
    }
}
