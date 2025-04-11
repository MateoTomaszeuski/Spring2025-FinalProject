import React from 'react'

import './App.css'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom'
import { MissingPage } from './components/MissingPage'
import { MainPage } from './components/MainPage'
import { DemoPage } from './components/DemoPage'
import { DocsPage } from './components/DocsPage'
import { SignedInPage } from './components/SignedInPage'

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
        <Link to="/" className='headerNav'>
          <h1>Consilium</h1>
        </Link>

        <nav className='navElements'>
          <ul>
            <li><Link to="/" className='headerNav'>Home</Link></li>
            <li><Link to="/demo" className='headerNav'>Demo</Link></li>
            <li><Link to="/docs" className='headerNav'>Docs</Link></li>
            <li><a id="download-button" href='https://youtu.be/dQw4w9WgXcQ?si=cxQcwxCL5z5MN--Q' target='_blank'>Download Now!</a></li>
          </ul>
        </nav>
      </header>
    )
  }
}

export default App
