class DataGrid {
    _dotnetRef;
    _id;
    _options;

    initialize(dotNetRef, id) {
        this._dotnetRef = dotNetRef;
        this._id = id;

        this.#setHeaderPadding();
    }

    dispose() {
    }

    #setHeaderPadding() {
        let el = document.querySelector(`#${this._id}>.data-grid-header`);

        if (el) {
            el.style.paddingInlineEnd = `${this.#getScrollbarWidth()}px`;
        }
    }

    #getScrollbarWidth() {
        // Create the measurement node
        var scrollDiv = document.createElement("div");
        scrollDiv.className = "scrollbar-measure";
        document.body.appendChild(scrollDiv);

        // Get the scrollbar width
        var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth;

        // Delete the DIV 
        document.body.removeChild(scrollDiv);

        return scrollbarWidth;
    }
}

export function getDataGrid() {
    return new DataGrid();
}