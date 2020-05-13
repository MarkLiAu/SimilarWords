import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

  render() {
    return (
        <div >
        <NavMenu />
            {this.props.children}
       
      </div>
    );
  }
}
