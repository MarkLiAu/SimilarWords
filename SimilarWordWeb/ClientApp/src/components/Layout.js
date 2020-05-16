import React, { Component } from 'react';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

  render() {
    return (
      <div>
            <NavMenu />
            <div style={{ marginLeft: 2+'px'}}>
                {this.props.children}
            </div>
      </div>
    );
  }
}
