local skynet = require "skynet"
require "skynet.manager"
local mongo = require "skynet.db.mongo"

local client

local _M = {}

function _M.connect(args)
	client = mongo.client(
		{
			host = args.host, 
            port = tonumber(args.port)
		}
	)

    if client then
        return true
    end
    return false
end

function _M.disconnect()
    client:disconnect()
end

function _M.insert(args)
    local db = client:getDB(args.database)
    local c = db:getCollection(args.collection)
    c:insert(args.doc)
end

function _M.insertBatch(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
	c:batch_insert(args.docs)
end

function _M.delete(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
	c:delete(args.selector, args.single)
end

function _M.drop(args)
	local db = client:getDB(args.database)
	local r = db:runCommand("drop", args.collection)
	return r
end

function _M.findOne(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
    local result = c:findOne(args.query, args.selector)
	return result
end

function _M.findAll(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
	local result = {}
	local cursor = c:find(args.query, args.selector)
	if args.skip ~= nil then
		cursor:skip(args.skip)
	end
	if args.limit ~= nil then
		cursor:limit(args.limit)
	end
	while cursor:hasNext() do
		local document = cursor:next()
		table.insert(result, document)
	end	
    cursor:close()

    if #result > 0 then
        return result
    end
end

function _M.update(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
	c:update(args.selector, args.update, args.upsert, args.multi)
end

function _M.createIndex(args)
	local db = client:getDB(args.database)
	local c = db:getCollection(args.collection)
	local result = c:createIndex(args.keys, args.option)
	return result
end

function _M.runCommand(args)
	local db = client:getDB(args.database)
	local result = db:runCommand(args)
	return result
end

return _M
