class Draggable {
    _dotnetRef;
    _id;
    _options;

    initialize(dotNetRef, id, options) {
        this._dotnetRef = dotNetRef;
        this._id = id;
        this._options = options;

        this.#getElement().setAttribute('draggable', 'true');
        this.#getElement().addEventListener('dragstart', this.#onDragStart.bind(this));
    }

    dispose() {
    }

    #getElement() {
        return document.getElementById(this._id);
    }

    #onDragStart(args) {
        Object.entries(this._options.dataTransferItems).forEach(e => {
            args.dataTransfer.setData(e[0], e[1]);
        })
        ev.dataTransfer.dropEffect = this._options.dropEffect;
        console.log(args);
    }
}

export function getDraggable() {
    return new Draggable();
}