--[[
Author: fasthro
Date: 2020-09-24 15:18:37
Description: 
--]]
local AS_TYPE = {
    ["GImage"] = "asImage",
    ["GComponent"] = "asCom",
    ["GButton"] = "asButton",
    ["GLabel"] = "asLabel",
    ["GProgressBar"] = "asProgress",
    ["GSlider"] = "asSlider",
    ["GComboBox"] = "asComboBox",
    ["GTextField"] = "asTextField",
    ["GRichTextField"] = "asRichTextField",
    ["GTextInput"] = "asTextInput",
    ["GLoader"] = "asLoader",
    ["GList"] = "asList",
    ["GGraph"] = "asGraph",
    ["GGroup"] = "asGroup",
    ["GMovieClip"] = "asMovieClip",
    ["GTree"] = "asTree",
    ["GTreeNode"] = "treeNode",
    ["GRoot"] = "root",
    ["GLoader3D"] = "asLoader3D"
}

local EXTENTION_TYPE = {
    ["Button"] = "asButton",
    ["ProgressBar"] = "asProgress",
    ["ComboBox"] = "asComboBox",
    ["Slider"] = "asSlider",
    ["Label"] = "asLabel"
}

local runner = fclass()

function runner:ctor(handler)
    self.handler = handler
    self.package = handler.pkg
    self.settings = handler.project:GetSettings("Publish").codeGeneration

    self.packageName = handler:ToFilename(handler.pkg.name)
    self.exportPath = handler.exportPath .. "/" .. self.packageName
    self.exportCodePath = handler.exportCodePath .. "/Automatic/Lua/FComponent/" .. self.packageName
    self.exportPanelCodePath = handler.exportCodePath .. "/Lua/Panel"
    self.definePanelPath = handler.exportCodePath .. "/Lua/Define/DefinePanel.lua"

    -- 不使用 FairyGUI 提供的类导出功能
    handler.genCode = false

    -- 是否为Resources Pakcage
    self.isResourcePackage = string.sub(self.packageName, 1, 3) == "RP_"
    if not self.isResourcePackage then
        handler.exportPath = self.exportPath
    else
        local _erp = ""
        local ps = utils.split(handler.exportPath, "\\")
        for k = 1, #ps - 2 do
            _erp = _erp .. ps[k] .. "\\"
        end
        _erp = _erp .. "UFramework\\Resources\\UI\\" .. self.packageName
        handler.exportPath = _erp
    end

    self.items = handler.items
    self.classes = handler:CollectClasses(self.settings.ignoreNoname, self.settings.ignoreNoname, "")

    self:_init()
end

function runner:_init()
    -- 清理目录
    utils.directoryClear(self.handler.exportPath)
    
    if not self.isResourcePackage then
        utils.directoryClear(self.exportCodePath)
    end
end

function runner:execute()
    if self.isResourcePackage then
        return
    end

    for i = 0, self.classes.Count - 1 do
        local class = self.classes[i]
        self:_genComponentCode(class)
        -- 根路径下的组件视为UIPanel
        if class.res.path == "/" then
            self:_genPanelCode(class)
        end
    end

    -- 生成 package 索引
    -- if self:_checkPackageEmpty() then
    --     self:_genPackageIndex()
    -- else
    --     utils.directoryDelete(self.exportCodePath)
    -- end

    self:_genPackageIndex()
    self:_updateDefinePanel()
end

function runner:_genComponentCode(info)
    -- 字段方法列表
    local methods = {}

    local writer = CodeFileWriter.new()
    writer:reset()
    writer:writeln("-- uframework automatically generated")
    writer:writeln(string.format("local component = fgui.extension_class(%s)", info.superClassName))
    -- ctor
    writer:writeln()
    writer:writeln("function component:ctor()")

    -- 有效字段
    local validCount = 0
    for i = 0, info.members.Count - 1 do
        local member = info.members[i]
        if self:_checkMemberName(member) then
            if member.type == "Controller" then
                writer:writelnTab(string.format("self.%s = self:GetControllerAt(%d)", member.name, member.index))
            elseif member.type == "Transition" then
                writer:writelnTab(string.format("self.%s = self:GetTransitionAt(%d)", member.name, member.index))
            else
                writer:writelnTab(string.format("self.%s = self:GetChildAt(%d).%s", member.name, member.index, self:_parseMemberType(member)))
            end

            local method = self:_getMemberMethod(member)
            if method ~= nil and method ~= "" then
                table.insert(methods, method)
            end

            validCount = validCount + 1
        end
    end

    writer:writeln("end")
    writer:writeln()

    -- method
    for k = 1, #methods do
        writer:writeln(methods[k])
    end

    writer:writeln(string.format('fgui.register_extension("ui://%s%s", component)', self.package.id, info.resId))
    writer:writeln()
    writer:writeln("return component")

    if validCount > 0 then
        writer:save(self.exportCodePath .. "\\" .. info.className .. ".lua")
    end
end

-- 检查child名字是否需要导出代码
-- 下划线开头的列为需要导出格式
function runner:_checkMemberName(member)
    return string.sub(member.name, 1, 1) == "_"
end

-- 类型转换
function runner:_parseMemberType(member)
    local ts = utils.split(member.type, ".")
    local key = ts[#ts]
    local type = AS_TYPE[key]
    if not type then
        if member.res ~= nil then
            local res = member.res
            local filePath = res.owner.basePath .. res.path .. "/" .. res.name .. ".xml"
            local xml = CS.FairyEditor.XMLExtension.Load(filePath)
            local extention = xml:GetAttribute("extention")
            if extention ~= nil then
                return EXTENTION_TYPE[extention] or "extention type:" .. extention
            end
            return "asCom"
        end
        return ""
    end
    return type
end

-- 字段属性函数
function runner:_getMemberMethod(member)
    local writer = CodeFileWriter.new()
    writer:reset()

    if member.type == "Controller" then
        -- setIndex
        writer:writeln(string.format("function component:setIndex%s(index)", member.name))
        writer:writelnTab(string.format("self.%s:SetSelectedIndex(index)", member.name))
        writer:writeln("end")
    elseif member.type == "Transition" then
    else
        local asType = self:_parseMemberType(member)
        if asType == "asButton" then
            writer:writeln(string.format("function component:setClick%s(callback, target)", member.name))
            writer:writelnTab(string.format("self.%s.onClick:Set(callback, target)", member.name))
            writer:writeln("end")
        elseif asType == "asTextField" or asType == "asRichTextField" or asType == "asTextInput" then
            writer:writeln(string.format("function component:setText%s(text)", member.name))
            writer:writelnTab(string.format("self.%s.text = text", member.name))
            writer:writeln("end")
        elseif asType == "asLoader" then
            writer:writeln(string.format("function component:setUrl%s(url)", member.name))
            writer:writelnTab(string.format("self.%s.url = url", member.name))
            writer:writeln("end")
        elseif asType == "asList" then
            writer:writeln(string.format("function component:initVirtualList%s(renderCallback, clickCallback, target)", member.name))
            writer:writelnTab(string.format("self.%s:SetVirtual()", member.name))
            writer:writelnTab(string.format("self.%s.itemRenderer = renderCallback", member.name))
            writer:writelnTab(string.format("self.%s.onClickItem:Set(clickCallback, target)", member.name))
            writer:writelnTab(string.format("self.%s.numItems = 0", member.name))
            writer:writeln("end")
            writer:writeln()
            writer:writeln(string.format("function component:setVirtualList%s(num)", member.name))
            writer:writelnTab(string.format("self.%s.numItems = num", member.name))
            writer:writeln("end")
        elseif asType == "asProgress" then
            writer:writeln(string.format("function component:setValue%s(value, max)", member.name))
            writer:writelnTab(string.format("self.%s.value = value", member.name))
            writer:writelnTab(string.format("self.%s.max = max", member.name))
            writer:writeln("end")
        end
    end
    return writer:tostring()
end

function runner:_genPanelCode(info)
    --
    local _panelName = info.className .. "Panel"
    local _panelPath = self.exportPanelCodePath .. "/" .. _panelName .. ".lua"

    if utils.fileExists(_panelPath) then
        return
    end

    local writer = CodeFileWriter.new()
    writer:reset()

    writer:writeln("local panel = typesys.def." .. _panelName .. "{")
    writer:writelnTab("__super = typesys.BasePanel,")
    writer:writeln("}")
    writer:writeln()

    writer:writeln("function panel:__ctor()")
    writer:writelnTab('self._name = "' .. info.className .. '"')
    writer:writelnTab('self._package = "' .. self.packageName .. '"')
    writer:writelnTab("self._layer = PANEL_LAYER.PANEL")
    writer:writelnTab("self._dependences = nil")
    writer:writeln()
    writer:writelnTab("panel.__super.__ctor(self)")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:_onAwake()")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:_onDispose()")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:onShow()")
    writer:writelnTab("panel.__super.onShow(self)")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:onHide()")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:onEventReceive(id)")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("function panel:onNetworReceive()")
    writer:writeln("end")
    writer:writeln()

    writer:writeln("return panel")

    writer:save(_panelPath)
end

function runner:_updateDefinePanel()
    local writer = CodeFileWriter.new()
    writer:reset()
    writer:writeln("-- uframework automatically generated")

    local files = utils.directoryGetFiles(self.exportPanelCodePath, "*.lua")
    for k = 0, files.Length - 1 do
        local fn = utils.fileName(files[k])
        writer:writeln(string.format('require("Panel.%s")', fn))
    end
    writer:save(self.definePanelPath)
end

function runner:_checkPackageEmpty()
    return utils.directoryGetFiles(self.exportCodePath, "*.lua").Length > 0
end

function runner:_genPackageIndex()
    local writer = CodeFileWriter.new()
    writer:reset()
    writer:writeln("-- uframework automatically generated")

    local files = utils.directoryGetFiles(self.exportCodePath, "*.lua")
    for k = 0, files.Length - 1 do
        local fn = utils.fileName(files[k])
        writer:writeln(string.format('require("FComponent.%s.%s")', self.packageName, fn))
    end
    writer:save(string.format("%s/%s", self.exportCodePath, "PackageComponent.lua"))
end

return runner
