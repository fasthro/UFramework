--[[
Author: fasthro
Date: 2020-09-24 15:18:37
Description: utils
--]]
local File = CS.System.IO.File
local Directory = CS.System.IO.Directory
local Path = CS.System.IO.Path

utils = {}

function utils.directoryCreate(dir)
    if not utils.directoryExists(dir) then
        Directory.CreateDirectory(dir)
    end
end

function utils.directoryDelete(dir)
    if utils.directoryExists(dir) then
        Directory.Delete(dir, true)
    end
end

function utils.directoryClear(dir)
    utils.directoryDelete(dir)
    utils.directoryCreate(dir)
end

function utils.directoryExists(dir)
    return Directory.Exists(dir)
end

function utils.directoryGetFiles(dir, pattern)
    if not utils.directoryExists(dir) then
        return nil
    end
    if pattern == nil then
        pattern = "*.*"
    end
    return Directory.GetFiles(dir, pattern)
end

function utils.fileExists(path)
    return File.Exists(path)
end

function utils.fileDelete(path)
    if File.Exists(path) then
        File.Delete(path)
    end
end

function utils.fileCreateText(path, content)
    utils.fileDelete(path)
    File.WriteAllText(path, content);
end

function utils.fileName(path, ext)
    if ext then
        return Path.GetFileName(path)
    end
    return Path.GetFileNameWithoutExtension(path)
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
