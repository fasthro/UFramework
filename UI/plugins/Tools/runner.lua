--[[
Author: fasthro
Date: 2020-10-05 17:35:23
Description: 
--]]

local tools = fclass()

function tools:ctor()
end

-- 创建包
-- @parame isCommonPackage 是否为公共包
function tools:createPackage(isCommonPackage)
    local packageName = "NewPackage"
    if isCommonPackage then
        packageName = "_" .. packageName
    end
    local package = App.project:GetPackageByName(packageName)
    if package ~= nil then
        return
    end
    package = App.project:CreatePackage(packageName)
    package:CreateFolder("Components", "/", true)
    package:CreateFolder("Images", "/", true)
    package:CreateFolder("Design", "/", true)
end

return tools