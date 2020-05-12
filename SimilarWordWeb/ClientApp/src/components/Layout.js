import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

  render() {
    return (
        <Grid fluid >
        <NavMenu />
        <Row>
            {this.props.children}
        </Row>
      </Grid>
    );
  }
}
