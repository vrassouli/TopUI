class Droppable {
    _dotnetRef;
    _id;
    _options;

    initialize(dotNetRef, id, options) {
        this._dotnetRef = dotNetRef;
        this._id = id;
        this._options = options;

    }

    dispose() {
    }

    #getElement() {
        return document.getElementById(this._id);
    }
}

export function getDroppable() {
    return new Droppable();
}