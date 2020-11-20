--[[
Author: fasthro
Date: 2020-11-13 15:33:06
Description: function
--]]
function _G.__parsedtn(dn)
    if type(dn) == "table" then
        return dn.__type_name, true
    end
    return dn, false
end

function _G.__newdtn(dtn, ...)
    local _name, _dt = __parsedtn(dtn)
    if _dt then
        return typesys.new(dtn, ...), _name
    end
    return assert(typesys.new(typesys[_name], ...), string.format("%s type undefined!", _name)), _name
end

function _G.__newdt(dt, ...)
    local _name, _dt = __parsedtn(dt)
    if _dt then
        return typesys.new(dt, ...)
    end
    return assert(typesys.new(typesys[_name], ...), string.format("%s type undefined!", _name))
end

function _G.__log(data)
    print(data)
end

function _G.__error(data)
    error(data)
end