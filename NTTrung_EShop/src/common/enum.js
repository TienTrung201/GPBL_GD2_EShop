const Enum = {
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
            Path: '/inventory-form',
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
    },
    ButtonType: {
        Pri: 'pri',
        IconPri: 'i-pri',
        Sec: 'sec',
        Link: 'link',
        Icon: 'icon',
        Danger: 'danger',
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
};
export default Enum;
