import React, {Component } from 'react';
// import './App.scss';
import {Nav,Navbar,NavDropdown,Form,FormControl,Button, Panel,PanelGroup } from 'react-bootstrap';
export class bootstrapTest1 extends Component {
    render() {
        return (
            <PanelGroup accordion id="accordion-example">
                <Panel eventKey="1">
                    <Panel.Heading>
                        <Panel.Title toggle>Collapsible Group Item #1</Panel.Title>
                    </Panel.Heading>
                    <Panel.Body collapsible>
                        Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry
                        richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard
                        dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf
                        moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla
                        assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore
                        wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur
                        butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim
                        aesthetic synth nesciunt you probably haven't heard of them accusamus
                        labore sustainable VHS.
    </Panel.Body>
                </Panel>
                <Panel eventKey="2">
                    <Panel.Heading>
                        <Panel.Title toggle>Collapsible Group Item #2</Panel.Title>
                    </Panel.Heading>
                    <Panel.Body collapsible>
                        Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry
                        richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard
                        dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf
                        moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla
                        assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore
                        wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur
                        butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim
                        aesthetic synth nesciunt you probably haven't heard of them accusamus
                        labore sustainable VHS.
    </Panel.Body>
                </Panel>
                <Panel eventKey="3">
                    <Panel.Heading>
                        <Panel.Title toggle>Collapsible Group Item #3</Panel.Title>
                    </Panel.Heading>
                    <Panel.Body collapsible>
                        Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry
                        richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard
                        dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf
                        moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla
                        assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore
                        wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur
                        butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim
                        aesthetic synth nesciunt you probably haven't heard of them accusamus
                        labore sustainable VHS.
    </Panel.Body>
                </Panel>
            </PanelGroup>
        )
    }

    renderZZ() {
        return (
            <div >
            <header>
                <Navbar expand="lg" variant="dark" bg="dark">
                    <Navbar.Brand href="#home">React-Bootstrap</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="mr-auto">
                            <Nav.Link href="#home">Home</Nav.Link>
                            <Nav.Link href="#link">Link</Nav.Link>
                            <NavDropdown title="Dropdown" id="basic-nav-dropdown">
                                <NavDropdown.Item>Action</NavDropdown.Item>
                                <NavDropdown.Item>Another action</NavDropdown.Item>
                                <NavDropdown.Item>Something</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item href="#action/3.4">Separated link</NavDropdown.Item>
                            </NavDropdown>
                        </Nav>
                        <Form inline>
                            <FormControl type="text" placeholder="Search" className="mr-sm-2" />
                            <Button variant="outline-success">Search</Button>
                        </Form>
                    </Navbar.Collapse>
                </Navbar>
            </header>
        </div>
        )
    }

}