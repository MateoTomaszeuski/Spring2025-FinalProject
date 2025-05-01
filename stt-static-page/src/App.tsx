import React from 'react'

import './App.css'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom'
import { MissingPage } from './components/MissingPage'
import { MainPage } from './components/MainPage'
import { DemoPage } from './components/DemoPage'
import { DocsPage } from './components/DocsPage'
import { SignedInPage } from './components/SignedInPage'
import { ValidatePage } from './components/ValidatePage'

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
              <Route path='/docs' element={<DocsPage />} />
              <Route path='/signedin' element={<SignedInPage />} />
              <Route path='/validate' element={<ValidatePage />} />
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
        <div className="header-content">

          <Link to="/" className='headerNav'>
            <div className="logo-title">
              <img src="/consilium-logo-white.svg" alt="Consilium logo" className="logo" />
              <h1 id="header-title">onsilium</h1>
            </div>
          </Link>

          <nav className='navElements'>
            <ul>
              <li><Link to="/" className='headerNav'>Home</Link></li>
              <li><Link to="/demo" className='headerNav'>Demo</Link></li>
              <li><Link to="/docs" className='headerNav'>Docs</Link></li>
              <li><a className="download-button" href='https://youtu.be/dQw4w9WgXcQ?si=cxQcwxCL5z5MN--Q' target='_blank'>Download Now!</a></li>
            </ul>
          </nav>
        </div>
      </header>
    )
  }
}

export default App
