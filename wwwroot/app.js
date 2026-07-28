window.focusRow = function (id) {
    const fila = document.getElementById(id);
    if (fila) {
        fila.scrollIntoView({ behavior: "smooth", block: "nearest" });
        fila.focus();
    }
};

window.toggleSidebar = function () {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');
    if (sidebar) {
        sidebar.classList.toggle('open');
    }
    if (overlay) {
        overlay.classList.toggle('open');
    }
};

window.showModal = function (modalId) {
    const el = document.getElementById(modalId);
    if (el && window.bootstrap && window.bootstrap.Modal) {
        const modal = window.bootstrap.Modal.getOrCreateInstance(el);
        modal.show();
    }
};

window.hideModal = function (modalId) {
    const el = document.getElementById(modalId);
    if (el && window.bootstrap && window.bootstrap.Modal) {
        const modal = window.bootstrap.Modal.getInstance(el);
        if (modal) {
            modal.hide();
        }
    }
};