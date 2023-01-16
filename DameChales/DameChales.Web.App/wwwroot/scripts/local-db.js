let db;

window.LocalDb = {
    Initialize: function () {
        db = new Dexie('damechales_database');
        db.version(1).stores({
            orders: 'id',
            foods: 'id',
        });
    },
    GetAll: async function (tableName) {
        return await db.table(tableName).toArray();
    },
    GetById: async function (id) {
        let order = await db.table(tableName).get(id);
        return order;
    },
    Insert: function (tableName, entity) {
        db.table(tableName).put(entity);
    },
    Remove: async function (tableName, id) {
        await db.table(tableName).bulkDelete([id]);
    }
};
