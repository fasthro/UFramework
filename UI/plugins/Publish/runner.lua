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
    ["GLoader3D"] = "asLoader3D",
}

local EXTENTION_TYPE = {
    ["Button"] = "asButton",
    ["ProgressBar"] = "asProgress",
    ["ComboBox"] =  "asComboBox",
    ["Slider"] = "asSlider",
    ["Label"] = "asLabel",
}

local runner = fclass()

function runner:ctor(handler)
    self.handler = handler
    self.package = handler.pkg
    self.settings = handler.project:GetSettings("Publish").codeGeneration

    self.packageName = handler:ToFilename(handler.pkg.name)
    self.exportPath = handler.exportPath .. '/'.. self.packageName
    self.exportCodePath = handler.exportCodePath .. '/'.. self.packageName

    handler.genCode = true
    handler.exportPath = self.exportPath
    
    self.items = handler.items
    self.classes = handler:CollectClasses(self.settings.ignoreNoname, self.settings.ignoreNoname, "")

    self:_init()
end

function runner:_init()
    -- 清理目录
    utils.directoryClear(self.exportPath)
    utils.directoryClear(self.exportCodePath)
end

function runner:execute()
    for i = 0, self.classes.Count - 1 do
        local class = self.classes[i];
        self:_genComponentCode(class)
        self:_genUICode(class)
    end
end

function runner:_genComponentCode(info)
    -- fprint("superClassName: " .. info.superClassName)
    -- fprint("className: " .. info.className)

    for i = 0, info.members.Count - 1 do
        local member = info.members[i]
        if self:_checkMemberName(member.name) then
            fprint(member.name .. " = " .. self:_parseMemberType(member))
        end
    end
end

function runner:_genUICode(info)

end

-- 检查child名字是否需要导出代码
-- 下划线开头的列为需要导出格式
function runner:_checkMemberName(name)
    return string.sub(name, 1, 1) == "_"
end

-- 类型转换
function runner:_parseMemberType(member)
    local ts = utils.split(member.type, ".")
    local key = ts[#ts]
    local type = AS_TYPE[key]
    if not type then
        if member.res ~= nil then
            local res = member.res
            local filePath = res.owner.basePath .. res.path .. "/" .. res.name ..".xml"
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

return runner