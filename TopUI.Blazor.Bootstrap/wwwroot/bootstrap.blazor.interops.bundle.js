class TopUiBootstrap {
    setTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
    }
    getTheme() {
        if (!document.documentElement.hasAttribute('data-bs-theme'))
            return 'light';

        return document.documentElement.getAttribute('data-bs-theme');
    }
    toggleTheme() {
        if (getTheme() === 'light')
            setTheme('dark');
        else
            setTheme('light');
    }

    setDir(dir) {
        document.documentElement.setAttribute('dir', dir);
    }
    getDir() {
        if (!document.documentElement.hasAttribute('dir'))
            return 'ltr';

        return document.documentElement.getAttribute('dir');
    }
    toggleDir() {
        if (getTheme() === 'ltr')
            setTheme('rtl');
        else
            setTheme('ltr');
    }

    showModal(dotNetRef) {
        const el = document.getElementById('bootstrapModal');
        if (el) {
            const modal = new bootstrap.Modal('#bootstrapModal');

            el.addEventListener('hidden.bs.modal', event => {
                dotNetRef.invokeMethodAsync('OnDialogHide');
            })
            modal.show();
        }
    }
    closeModal() {
        const el = document.getElementById('bootstrapModal');
        if (el) {
            const modal = bootstrap.Modal.getInstance(el);
            if (modal)
                modal.hide();
        }
    }

    showOffcanvas(dotNetRef) {
        const el = document.getElementById('bootstrapOffcanvas');
        if (el) {
            const offcanvas = new bootstrap.Offcanvas('#bootstrapOffcanvas');

            el.addEventListener('hidden.bs.offcanvas', event => {
                dotNetRef.invokeMethodAsync('OnOffcanvasHide');
            })
            offcanvas.show();
        }
    }
    closeOffcanvas() {
        const el = document.getElementById('bootstrapOffcanvas');
        if (el) {
            const offcanvas = bootstrap.Offcanvas.getInstance(el);
            if (offcanvas)
                offcanvas.hide();
        }
    }

    showToast(dotNetRef, id) {
        const el = document.getElementById(id);
        if (el) {
            const toast = new bootstrap.Toast(`#${id}`);

            el.addEventListener('hidden.bs.toast', event => {
                dotNetRef.invokeMethodAsync('OnToastHide', id);
            })
            toast.show();
        }
    }
    closeToast(id) {
        const el = document.getElementById(id);
        if (el) {
            const toast = bootstrap.Toast.getInstance(el);
            if (toast)
                toast.hide();
        }
    }
};
export function getTopUiBootstrap() {
    return new TopUiBootstrap();
}