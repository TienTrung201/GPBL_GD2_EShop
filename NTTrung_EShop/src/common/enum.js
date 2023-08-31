const Enum = {
    LocalStorage: {
        LangCode: 'lang',
    },
    ApiMethod: {
        GET: 1,
        POST: 2,
        PUT: 3,
        DELETE: 4,
    },
    EditMode: {
        None: 1,
        Add: 2,
        Update: 3,
        Delete: 4,
        Copy: 5,
    },
    Gender: {
        Male: 1,
        Female: 0,
        Other: 2,
    },
    Keyboard: {
        Up: 38,
        Down: 40,
        Enter: 13,
        S: 83,
        ESC: 27,
        D: 68,
        Insert: 45,
        Delete: 46,
        Ctrl1: 49,
        Comma: 188,
        BackSpace: 8,
    },
    Language: {
        VN: 'VN',
        EN: 'EN',
    },
    TypeDataTable: {
        Gender: 0,
        Date: 1,
        Money: 2,
        Default: 3,
        Show: 4,
        Business: 5,
        Code: 6,
    },
    KeyboardShortcuts: {
        CtrlShiftS: 'Ctrl + Shift + S',
        CtrlS: 'Ctrl + S',
        Esc: 'Esc',
        CtrlD: 'Ctrl + D',
        Ctrl1: 'Ctrl + 1',
        Enter: 'Enter',
    },
    Router: {
        Employee: {
            Name: 'employee',
            Path: '/employee',
        },
        Position: {
            Name: 'position',
            Path: '/position',
        },
        Department: {
            Name: 'department',
            Path: '/department',
        },
        Home: {
            Name: 'home',
            Path: '/',
        },
        Inventory: {
            Name: 'inventory',
            Path: '/inventory',
            Api: '/inventories',
        },
        InventoryForm: {
            Name: 'inventory-form',
            NameDetail: 'inventory-form-detail',
            Path: '/inventory-form',
            PathDetail: ':id',
        },
        ItemCategory: {
            Name: 'item-category',
            Path: '/item-category',
            Api: '/ItemCategories',
        },
        Unit: {
            Name: 'unit',
            Path: '/unit',
            Api: '/Units',
        },
        Picture: {
            Name: 'unit',
            Path: '/unit',
            Api: '/Pictures',
        },
    },
    ErorCode: {
        ExistedConstrain: 1003, //Có phát snh dữ liệu liên quan
        DuplicateCode: 1002, //Trùng mã
        NotFoundCode: 1001, //Không tìm thấy
        ErrorServerCode: 1000, //Lỗi Server
    },
    RegExp: {
        Code: /[A-Za-z]{1,2}-\d{4,}/g,
        Guid: /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/,
    },
    ButtonType: {
        Pri: 'pri',
        IconPri: 'i-pri',
        Sec: 'sec',
        Link: 'link',
        Icon: 'icon',
        Danger: 'danger',
    },
    Icon: {
        Success: 'success',
        Error: 'error',
        Info: 'info',
    },
    AlignColumn: {
        Center: 0,
        Right: 1,
        Left: 2,
    },
    AlignColumnFe: {
        Center: 'center',
        Right: 'right',
        Left: 'left',
    },
    Sort: {
        Desc: 1,
        Asc: 2,
    },
    FilterBy: {
        Equal: 1,
        NotEqual: 2,
        Contain: 3,
        NotContain: 4,
        Greater: 5,
        Smaller: 6,
        AllData: 7,
        All: 'all',
        True: '1',
        False: '0',
    },
    TypeProperties: {
        Color: 'color',
        Size: 'size',
    },
    MaxLength: {
        Money: 18,
        Code: 20,
        Default: 100,
        Max: 255,
    },
};
export default Enum;
