--[[
Author: fasthro
Date: 2020-09-24 15:18:37
Description: utils
--]]
local _file = CS.System.IO.File
local _directory = CS.System.IO.Directory

utils = {}

function utils.directoryCreate(dir)
    if not utils.directoryExists(dir) then
        _directory.CreateDirectory(dir)
    end
end

function utils.directoryDelete(dir)
    if utils.directoryExists(dir) then
        _directory.Delete(dir, true)
    end
end

function utils.directoryClear(dir)
    utils.directoryDelete(dir)
    utils.directoryCreate(dir)
end

function utils.directoryExists(dir)
    return _directory.Exists(dir)
end

function utils.directoryGetFiles(dir, pattern)
    if not utils.directoryExists(dir) then
        return nil
    end
    return _directory.GetFiles(dir, pattern)
end

function utils.split(str, delimiter)
	if (delimiter == '') then return false end
	local pos, arr = 0, {}
	for st, sp in function() return string.find(str, delimiter, pos, true) end do
		table.insert(arr, string.sub(str, pos, st - 1))
		pos = sp + 1
	end
	table.insert(arr, string.sub(str, pos))
	return arr
end