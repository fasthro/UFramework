--[[
Author: fasthro
Date: 2020-09-25 10:33:20
Description: fairy
--]]

EventContext = FairyGUI.EventContext
EventListener = FairyGUI.EventListener
EventDispatcher = FairyGUI.EventDispatcher
InputEvent = FairyGUI.InputEvent
NTexture = FairyGUI.NTexture
Container = FairyGUI.Container
Image = FairyGUI.Image
Stage = FairyGUI.Stage
Controller = FairyGUI.Controller
GObject = FairyGUI.GObject
GGraph = FairyGUI.GGraph
GGroup = FairyGUI.GGroup
GImage = FairyGUI.GImage
GLoader = FairyGUI.GLoader
GMovieClip = FairyGUI.GMovieClip
TextFormat = FairyGUI.TextFormat
GTextField = FairyGUI.GTextField
GRichTextField = FairyGUI.GRichTextField
GTextInput = FairyGUI.GTextInput
GComponent = FairyGUI.GComponent
GList = FairyGUI.GList
GRoot = FairyGUI.GRoot
GLabel = FairyGUI.GLabel
GButton = FairyGUI.GButton
GComboBox = FairyGUI.GComboBox
GProgressBar = FairyGUI.GProgressBar
GSlider = FairyGUI.GSlider
PopupMenu = FairyGUI.PopupMenu
ScrollPane = FairyGUI.ScrollPane
Transition = FairyGUI.Transition
UIPackage = FairyGUI.UIPackage
Window = FairyGUI.Window
GObjectPool = FairyGUI.GObjectPool
Relations = FairyGUI.Relations
RelationType = FairyGUI.RelationType
UIPanel = FairyGUI.UIPanel
UIPainter = FairyGUI.UIPainter
TypingEffect = FairyGUI.TypingEffect
GTween = FairyGUI.GTween
GTweener = FairyGUI.GTweener
EaseType = FairyGUI.EaseType

fgui = {}

function fgui.window_class(base)
    local o = {}

    local base = base or FairyGUI.Window
    setmetatable(o, base)

    o.__index = o
    o.base = base

    o.New = function(...)
        local t = {}
        setmetatable(t, o)

        local ins = FairyGUI.Window.New()
        tolua.setpeer(ins, t)
        ins:SetLuaPeer(t)
        if t.ctor then t.ctor(ins, ...) end

        return ins
    end

    return o
end

function fgui.register_extension(url, extension)
    FairyGUI.UIObjectFactory.SetExtension(url, typeof(extension.base),extension.Extend)
end

function fgui.extension_class(base)
    local o = {}
    o.__index = o

    o.base = base or GComponent

    o.Extend = function(ins)
        local t = {}
        setmetatable(t, o)
        tolua.setpeer(ins, t)
        return t
    end

    return o
end
