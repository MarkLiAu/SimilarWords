import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem , NavDropdown, MenuItem} from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar className='navbar-custom' inverse fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link title="V20.5.31" to={'/'}>SimilarWord</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/wordmemory'}>
                <NavItem>
                    <Glyphicon glyph='th-list' />Memory
                </NavItem>
            </LinkContainer>
            <NavDropdown eventKey={3} title="Admin" id="basic-nav-dropdown">
                <LinkContainer to={{ pathname: '/admin/reload', state: { cmd: 'reload' } }} >
                    <MenuItem eventKey={3.1}>reload wordlist</MenuItem>
                </LinkContainer>

               <LinkContainer to={{ pathname: '/admin/resetmemory', state: { cmd: 'resetmemory' } }} >
                    <MenuItem eventKey={3.2}>Reset Memory</MenuItem>
                </LinkContainer>

                <LinkContainer to={'/MemoryLog'}>
                    <MenuItem eventKey={3.6}>Memory Log</MenuItem>
                </LinkContainer>

            </NavDropdown>

         </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
