function toggleHeader() {
    let header = document.querySelector('.header');
    let printButton = document.querySelector('.print-button');
    header.style.display = 'none';
    printButton.style.display = 'none';
    window.print();
    header.style.display = 'flex';
    printButton.style.display = 'inline-block';
}