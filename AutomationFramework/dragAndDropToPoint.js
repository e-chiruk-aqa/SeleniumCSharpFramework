var args = arguments,
    elemSrc = args[0],
    targetX = args[1],
    targetY = args[2],
    delay = args[3] || 1,
    key = args[4] || '',
    alt = key === 'alt' || key === '\uE00A',
    ctrl = key === 'ctrl' || key === '\uE009',
    shift = key === 'shift' || key === '\uE008',
    doc = elemSrc.ownerDocument,
    box1 = elemSrc.getBoundingClientRect(),
    x = box1.left + (box1.width / 2),
    y = box1.top + (box1.height / 2),
    source = doc.elementFromPoint(x, y);

if (!source) {
    var ex = new Error('source or target element is not interactable');
    ex.code = 15;
    throw ex;
}

var dataTransfer = {
    constructor: DataTransfer,
    effectAllowed: null,
    dropEffect: null,
    types: [],
    files: Object.setPrototypeOf([], null),
    _items: Object.setPrototypeOf([], {
        add: function add(data, type) {
            this[this.length] = {
                _data: '' + data,
                kind: 'string',
                type: type,
                getAsFile: function () { },
                getAsString: function (callback) { callback(this._data) }
            };
            dataTransfer.types.push(type);
        },
        remove: function remove(i) {
            Array.prototype.splice.call(this, i & 65535, 1);
            dataTransfer.types.splice(i & 65535, 1);
        },
        clear: function clear(data, type) {
            this.length = 0;
            dataTransfer.types.length = 0;
        }
    }),
    setData: function setData(format, data) {
        this.clearData(format);
        this._items.add(data, format);
    },
    getData: function getData(format) {
        for (var i = this._items.length; i-- && this._items[i].type !== format;);
        return i >= 0 ? this._items[i]._data : null;
    },
    clearData: function clearData(format) {
        for (var i = this._items.length; i-- && this._items[i].type !== format;);
        this._items.remove(i);
    },
    setDragImage: function setDragImage(format) { }
};

if ('items' in DataTransfer.prototype)
    dataTransfer.items = dataTransfer._items;

emit_mouse(source, ['pointerdown', 'mousedown'], 1, function () {
    var elem = source;
    while (elem && !elem.draggable)
        elem = elem.parentElement;
    if (!elem || !elem.contains(source))
        return;
    
    emit_drag(source, 'dragstart', delay, function () {
        x = targetX + 20;
        y = targetY + 20;
        emit_drag(source, 'dragenter', 1, function () {
            emit_drag(source, 'dragover', delay, function () {
                emit_drag(doc.elementFromPoint(x, y), 'drop', 1, function () {
                    emit_drag(source, 'dragend', 1, function () {
                        emit_mouse(doc.elementFromPoint(x, y), ['mouseup', 'pointerup']);
                    })
                })
            })
        })
    })
});


function emit_mouse(element, types, delay, callback) {
    for (var i = 0; i < types.length; ++i) {
        var event = doc.createEvent('MouseEvent');
        event.initMouseEvent(types[i], true, true, doc.defaultView, 0, 0, 0, x, y, ctrl, alt, shift, false, 0, null);
        element.dispatchEvent(event);
    }
    callback && setTimeout(callback, delay);
}

function emit_drag(element, type, delay, callback) {
    var event = doc.createEvent('DragEvent');
    event.initMouseEvent(type, true, true, doc.defaultView, 0, 0, 0, x, y, ctrl, alt, shift, false, 0, null);

    Object.setPrototypeOf(event, null);
    event.dataTransfer = dataTransfer;
    Object.setPrototypeOf(event, DragEvent.prototype);

    element.dispatchEvent(event);
    callback && setTimeout(callback, delay);
}
