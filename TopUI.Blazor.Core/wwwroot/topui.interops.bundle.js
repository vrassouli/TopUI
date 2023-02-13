
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

class Dragger {
    _id;
    _dotnetRef;
    _options;
    _pointerDown;
    _startPoint;
    _shadow;
    _shadowRect;

    initialize(dotNetRef, id, options) {
        this._dotnetRef = dotNetRef;
        this._id = id;
        this._options = options;

        this.#getElement().addEventListener('pointerdown', this.#onPointerDown.bind(this));
        document.addEventListener('pointermove', this.#onPointerMove.bind(this));
        document.addEventListener('pointerup', this.#onPointerUp.bind(this));
    }

    dispose() {
        this.#getElement().removeEventListener('pointerdown', this.#onPointerDown)
        document.removeEventListener('pointermove', this.#onPointerMove)
        document.removeEventListener('pointerup', this.#onPointerUp)
    }

    #getElement() {
        return document.getElementById(this._id);
    }

    #addShadow() {
        if (this._shadow)
            return this._shadow;

        this._shadowRect = this.#getElement().getBoundingClientRect();

        this._shadow = document.createElement('div');

        let cloned = this.#getElement().cloneNode(true);
        cloned.removeAttribute('id');
        this.#copyNodeStyle(this.#getElement(), cloned);

        this._shadow.appendChild(cloned);

        this._shadow.style.position = 'absolute';
        this._shadow.style.width = `${this._shadowRect.width}px`;
        this._shadow.style.height = `${this._shadowRect.height}px`;
        this._shadow.style.left = `${this._shadowRect.left}px`;
        this._shadow.style.top = `${this._shadowRect.top}px`;

        document.body.appendChild(this._shadow);
    }

    #removeShadow() {
        if (this._shadow) {
            this._shadow.remove();
            this._shadow = null;
            this._shadowRect = null;
        }
    }

    #isRtl() {
        return document.dir == 'rtl';
    }

    #moveShadow(delta) {
        if (this._shadow) {
            if (this.#isRtl())
                this._shadow.style.left = `${this._shadowRect.left - delta.dx}px`;
            else
                this._shadow.style.left = `${this._shadowRect.left + delta.dx}px`;
            this._shadow.style.top = `${this._shadowRect.top + delta.dy}px`;
        }
    }

    #copyNodeStyle(sourceNode, targetNode) {
        const computedStyle = window.getComputedStyle(sourceNode);
        Array.from(computedStyle).forEach(key => targetNode.style.setProperty(key, computedStyle.getPropertyValue(key), computedStyle.getPropertyPriority(key)))
    }

    #getDelta(point) {
        var dx = 0;
        var dy = 0;

        if (this._options.allowHorizontal) {
            if (this.#isRtl())
                dx = this._startPoint.x - point.x;
            else
                dx = point.x - this._startPoint.x;
        }
        if (this._options.allowVertical) {
            if (this.#isRtl())
                dy = this._startPoint.y - point.y;
            else
                dy = point.y - this._startPoint.y;
        }

        return { dx, dy };
    }

    #onPointerDown(args) {
        this._pointerDown = true;
        this._startPoint = { x: args.clientX, y: args.clientY };

        this.#addShadow();

        args.stopPropagation();
    }

    #onPointerMove(args) {
        if (this._pointerDown) {

            var currentPoint = { x: args.clientX, y: args.clientY };
            var delta = this.#getDelta(currentPoint);

            this.#moveShadow(delta);
            this._dotnetRef.invokeMethodAsync('OnDragging', delta);
        }
    }

    #onPointerUp(args) {
        if (this._pointerDown) {
            var currentPoint = { x: args.clientX, y: args.clientY };
            var delta = this.#getDelta(currentPoint);

            this.#removeShadow();
            this._dotnetRef.invokeMethodAsync('OnDragged', delta);
        }

        this._pointerDown = false;
        this._startPoint = { x: 0, y: 0 }
    }
}
export function getDragger() {
    return new Dragger();
}
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
class ScrollSync {
    _source;
    _targets;
    _dotnetRef;
    _options;

    initialize(dotNetRef, source, targets, options) {
        this._dotnetRef = dotNetRef;
        this._source = source;
        this._targets = targets;
        this._options = options;

        this.#getSource().addEventListener('scroll', this.#onScroll.bind(this));
    }

    dispose() {
    }

    #getSource() {
        return document.querySelector(this._source);
    }

    #onScroll() {
        this._targets.forEach(t => {
            if (this._options.syncHorizontal)
                document.querySelector(t).scrollLeft = this.#getSource().scrollLeft;
            if (this._options.syncVertical)
                document.querySelector(t).scrollTop = this.#getSource().scrollTop;
        });
    }
}

export function getScrollSync() {
    return new ScrollSync();
}