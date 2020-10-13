--[[
Author: fasthro
Date: 2020-10-05 16:44:24
Description: 
--]]

local runner = require(PluginPath..'/runner')

local toolMenu = App.menu:GetSubMenu("tool");
toolMenu:AddItem("-> 创建包", "menu_id_1", function(menuItem)
    local tools = runner.new()    
    tools:createPackage(false)
end)

toolMenu:AddItem("-> 创建公共包", "menu_id_2", function(menuItem)
    local tools = runner.new()   
    tools:createPackage(true)
end)

-------do cleanup here-------

function onDestroy()
    toolMenu:RemoveItem("menu_id_1")
    toolMenu:RemoveItem("menu_id_2")
end