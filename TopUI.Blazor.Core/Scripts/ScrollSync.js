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