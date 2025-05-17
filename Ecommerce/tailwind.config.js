/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
      './Pages/**/*.{cshtml,html,js}',
      './Views/**/*.{cshtml,html,js}',
      './Components/**/*.{cs,razor}',
      './wwwroot/**/*.{html,js}',
      "./node_modules/flowbite/**/*.js"
    ],
    theme: {
      extend: {},
    },
    plugins: [
        require('flowbite/plugin')
    ],
  }