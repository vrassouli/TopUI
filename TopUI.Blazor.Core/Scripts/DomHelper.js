class DomHelper {
    invokeClickEvent(sourceSelector) {
        var source = document.querySelector(sourceSelector);

        if (source) {
            source.click();
        }
    }

    static async downloadFileFromStream(fileName, contentStreamReference) {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');

        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();

        URL.revokeObjectURL(url);
    }
}

export function getDomHelper() {
    return new DomHelper();
}