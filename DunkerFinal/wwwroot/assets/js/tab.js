const tabItems = document.querySelectorAll('.tab-link')
const tabs = document.querySelectorAll('.tab')

tabItems.forEach(tabItem => {
  tabItem.addEventListener('click', function (e) {
    e.preventDefault()

    const selectedTab = this.getAttribute('data-tab')

    tabs.forEach(tab => {
      if (tab.getAttribute('data-tab') === selectedTab) {
        tab.classList.add('active', 'd-block')
        tab.classList.remove('d-none')
      } else {
        tab.classList.remove('active', 'd-block')
        tab.classList.add('d-none')
      }
    })
  })
})
