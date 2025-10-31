window.initializeDropdown = (id) =>
{
    window.addEventListener('load', () => adjustFixedDivPosition(id));
    window.addEventListener('scroll', () => adjustFixedDivPosition(id));
    window.addEventListener('resize', () => adjustFixedDivPosition(id));
    adjustFixedDivPosition(id);
}

function adjustFixedDivPosition(id) {
    const dropdownCover = document.getElementById(id);
    const dropdownTable = document.getElementsByClassName('dropdown-table-'+id)[0];
    const dropdownCoverBottom = dropdownCover.getBoundingClientRect().bottom;
    dropdownTable.style.top = `${dropdownCoverBottom}px`;
}