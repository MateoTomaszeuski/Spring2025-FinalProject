import React from 'react'
import './App.css'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom'
import { MissingPage } from './components/MissingPage'
import { MainPage } from './components/MainPage'
import { DemoPage } from './components/DemoPage'

class App extends React.Component {
  render() {
    return (
      <div id='app'>
        <BrowserRouter>
          <Header />
          <div id='content'>
            <Routes>
              <Route path='/' element={<MainPage />} />
              <Route path='/demo' element={<DemoPage />} />
              <Route path='*' element={<MissingPage />} />
            </Routes>
          </div>
        </BrowserRouter>
      </div>
    )
  }
}

class Header extends React.Component {
  render() {
    return (
      <header>
        <Link to="/" className='headerNav'><h1>Consilium</h1></Link>
          {/* Update here with a clickable link to home as well */}
        <div className='navElements'>
          <Link to="/" className='headerNav'>Home</Link>
          <Link to="/demo" className='headerNav'>Demo</Link>
        </div>
        <a href='https://youtu.be/dQw4w9WgXcQ?si=cxQcwxCL5z5MN--Q' target='_blank'>Download Now!</a>
      </header>
    )
  }
}

export default App
