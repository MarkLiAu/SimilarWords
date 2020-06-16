import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem , NavDropdown, MenuItem} from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { GetLoginUser } from './CommTools';
import './NavMenu.css';


const LoginMenu = () => {
    return (
        <LinkContainer to={'/login'}>
            <NavItem >
                <Glyphicon glyph='th-list' />Login
                </NavItem>
        </LinkContainer>
        )
}

const UserMenu = () => {
    return (
    <Fragment>
        <LinkContainer to={'/wordmemory'}>
            <NavItem>
                <Glyphicon glyph='th-list' />Study
                </NavItem>
        </LinkContainer>

        <NavDropdown eventKey={3} title="Admin" id="basic-nav-dropdown">
            <LinkContainer to={{ pathname: '/admin/reload', state: { cmd: 'reload' } }} >
                <MenuItem eventKey={3.1}>reload wordlist</MenuItem>
            </LinkContainer>

            <LinkContainer to={{ pathname: '/admin/resetmemory', state: { cmd: 'resetmemory' } }} >
                <MenuItem eventKey={3.2}>Reset Study data</MenuItem>
            </LinkContainer>

            <LinkContainer to={'/MemoryLog'}>
                <MenuItem eventKey={3.6}>Study Log</MenuItem>
            </LinkContainer>

                <LinkContainer to={{ pathname: '/admin/fixmemory', state: { cmd: 'fixmemory' } }} >
                <MenuItem eventKey={3.8}>Fix Study data</MenuItem>
            </LinkContainer>

            <LinkContainer to={{ pathname: '/admin/logout', state: { cmd: 'logout' } }} >
                <MenuItem eventKey={3.7}>Logout</MenuItem>
                </LinkContainer>

                <LinkContainer to={'/BootstrapTest1'}>
                    <MenuItem eventKey={3.9}> Test </MenuItem>
                </LinkContainer>

        </NavDropdown>
        </Fragment>
        )
}

export class NavMenu extends Component {
  displayName = NavMenu.name

    render() {
        let loginUser = GetLoginUser();
        console.log("in NaMenu, loginuser:", loginUser);
    return (
      <Navbar className='navbar-custom' inverse fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link title="V20.6.5" to={'/'}>SimilarWord</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            {loginUser === null ? <LoginMenu></LoginMenu>  : <UserMenu></UserMenu>  }
         </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
